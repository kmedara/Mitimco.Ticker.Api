using System.Globalization;
using System.Linq;
using System.Numerics;


namespace Ticker.Domain.Ticker
{
    /// <summary>
    /// Zero Dependencies or dependents
    /// Pure business logic relating to domain objects
    /// </summary>
    public sealed class TickerCalculationDomainService: ITickerCalculationDomainService
    {

        #region alpha
        public double CalculateAlpha(Dictionary<DateTime, OHLCV> historicalData, Dictionary<DateTime, OHLCV> benchmarkData, DateTime from, DateTime to, double riskFreeRate)
        {
            var historicalReturns = CalculateReturns(from, to , historicalData);
            var benchMarkReturns = CalculateReturns(from, to, benchmarkData);

            double _beta = CalculateBeta(historicalReturns, benchMarkReturns);


            double expected = CalculateExpectedReturn(_beta, riskFreeRate, benchMarkReturns);

            double actual = CalculateExpectedReturn(_beta, riskFreeRate, historicalReturns);

            return actual - expected;
        }
        #endregion

        #region beta
        public double CalculateBeta(List<DatePrice> historicalData, List<DatePrice> benchMarkData)
        {

            if (historicalData.Where(el => !benchMarkData.Any(bd => bd.Date == bd.Date)).Any())
            {
                throw new ArgumentException("Input arrays must have the same dates");
            }

            double covariance = Covariance(historicalData, benchMarkData);
            double marketVariance = Variance(benchMarkData);

            if (marketVariance == 0)
            {
                throw new InvalidOperationException("Market variance is zero. Beta cannot be calculated.");
            }

            double beta = covariance / marketVariance;

            return beta;
        }
        #endregion

        #region daily return
        public List<DatePrice> CalculateReturns(DateTime? from, DateTime? to, Dictionary<DateTime, OHLCV> timeSeries)
        {
            var results = new List<DatePrice>();
            var priceData = timeSeries
                .Where(item => item.Key >= from || from == null)
                .Where(item => item.Key <= to || to == null)
                .Select(item => (item.Key, item.Value.Close))
                .OrderByDescending(item => item.Key)
                ;

            //NumberFormatInfo percentageFormat = new NumberFormatInfo { PercentPositivePattern = 1, PercentNegativePattern = 1 };

            for (int i = 0; i < priceData.Count() - 1; i++)
            {
                double currentPrice = double.Parse(priceData.ElementAt(i).Close);
                double previousPrice = double.Parse(priceData.ElementAt(i + 1).Close);
                double returnPercentage = (currentPrice - previousPrice) / previousPrice;
                results.Add(new DatePrice(priceData.ElementAt(i).Key, returnPercentage));
            }

            return results;
        }
        #endregion
        #region expected return
        public double CalculateExpectedReturn(double beta, double riskFreeRate, List<DatePrice> benchMarkData)
        {
            var avgMarketReturn = benchMarkData.Select(el => el.Value).ToList().Average();
            return riskFreeRate + beta * (avgMarketReturn - riskFreeRate);
        }
        #endregion
        #region variance
        public double Variance(List<DatePrice> data)
        {

            double mean = data.Select(el => el.Value).ToList().Average();
            double variance = 0.0;
            foreach (var x in data)
            {
                variance += Math.Pow(x.Value - mean, 2);
            }

            variance /= (data.Count - 1); // Use (n - 1) for sample variance

            return variance;
        }
        #endregion

        #region covariance
        public double Covariance(List<DatePrice> historicalData, List<DatePrice> benchMarkData)
        {
            double avgSource = historicalData.Select(el => el.Value).Average();
            double avgOther = historicalData.Select(el => el.Value).Average();
            double covariance = 0;

            for (int i = 0; i < historicalData.Count; i++)
                covariance += (historicalData.ElementAt(i).Value - avgSource) * (benchMarkData.ElementAt(i).Value - avgOther);

            return covariance / historicalData.Count;
        }

        #endregion
    }
}
