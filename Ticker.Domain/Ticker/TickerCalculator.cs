using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using System.Globalization;
using System.Linq;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Ticker.Domain.Ticker
{
    /// <summary>
    /// Zero Dependencies or dependents
    /// Pure business logic relating to domain objects
    /// </summary>
    public sealed class TickerCalculator : ITickerCalculator
    {
        #region alpha
        public decimal Alpha(Dictionary<DateTime, OHLCV> historicalData, Dictionary<DateTime, OHLCV> benchmarkData, DateTime? from, DateTime? to, decimal riskFreeRate)
        {
            var historicalReturns = Returns(from, to, historicalData);
            var benchMarkReturns = Returns(from, to, benchmarkData);
            decimal _beta = Beta(historicalReturns, benchMarkReturns);
            decimal expected = ExpectedReturn(_beta, riskFreeRate, benchMarkReturns);
            decimal actual = ExpectedReturn(_beta, riskFreeRate, historicalReturns);
            return actual - expected;
        }
        #endregion

        #region beta
        public decimal Beta(List<DatePrice> historicalData, List<DatePrice> benchMarkData)
        {

            if (historicalData.Where(el => !benchMarkData.Any(bd => bd.Date == bd.Date)).Any())
            {
                throw new ArgumentException("Input arrays must have the same dates");
            }

            decimal covariance = Covariance(historicalData, benchMarkData);
            decimal marketVariance = Variance(benchMarkData);

            if (marketVariance == 0)
            {
                throw new InvalidOperationException("Market variance is zero. Beta cannot be calculated.");
            }

            return covariance / marketVariance;
        }
        #endregion

        #region return

        public List<DatePrice> Returns(DateTime? from, DateTime? to, Dictionary<DateTime, OHLCV> timeSeries)
        {
            var results = new List<DatePrice>();
            var priceData = timeSeries
                .Where(item => item.Key >= from || from == null)
                .Where(item => item.Key <= to || to == null)
                .Select(item => (item.Key, item.Value.Close))
                .OrderBy(item => item.Key);

            for (int i = 1; i < priceData.Count(); i++)
            {
                decimal previous = decimal.Parse(priceData.ElementAt(i - 1).Close);
                decimal current = decimal.Parse(priceData.ElementAt(i).Close);
                decimal returnPercentage = (current - previous) / previous;
                results.Add(new DatePrice(priceData.ElementAt(i).Key, returnPercentage.ToString()));
            }

            return results;
        }
        #endregion
        #region expected return
        public decimal ExpectedReturn(decimal beta, decimal riskFreeRate, List<DatePrice> benchMarkData)
        {
            var avgMarketReturn = benchMarkData.Select(el => decimal.Parse(el.Value)).ToList().Average();
            return riskFreeRate + beta * (avgMarketReturn - riskFreeRate);
        }
        #endregion
        #region variance
        public decimal Variance(List<DatePrice> data)
        {

            decimal mean = data.Select(el => decimal.Parse(el.Value)).ToList().Average();
            decimal variance = 0.0m;
            foreach (var x in data)
            {
                variance += (Decimal.Parse(x.Value) - mean) * (Decimal.Parse(x.Value) - mean);//th.Pow(Decimal.Parse(x.Value) - mean, 2.0);
            }

            return variance / data.Count;
        }
        #endregion

        #region covariance
        public decimal Covariance(List<DatePrice> historicalData, List<DatePrice> benchMarkData)
        {
            decimal avgSource = historicalData.Select(el => decimal.Parse(el.Value)).Average();
            decimal avgOther = benchMarkData.Select(el => decimal.Parse(el.Value)).Average();
            decimal covariance = 0;

            for (int i = 0; i < historicalData.Count; i++)
                covariance += (decimal.Parse(historicalData.ElementAt(i).Value) - avgSource) * (decimal.Parse(benchMarkData.ElementAt(i).Value) - avgOther);

            return covariance / historicalData.Count;
        }

        public void LinearRegression(decimal[] x, decimal[] y, out decimal rSquared, out decimal yIntercept, out decimal slope)
        {
            decimal sumOfX = 0;
            decimal sumOfY = 0;
            decimal sumOfXSq = 0;
            decimal sumOfYSq = 0;
            decimal sumCodeviates = 0;

            for (var i = 0; i < x.Length; i++)
            {
                var xVal = x[i];
                var yVal = y[i];
                sumCodeviates += xVal * yVal;
                sumOfX += xVal;
                sumOfY += yVal;
                sumOfXSq += xVal * xVal;
                sumOfYSq += yVal * yVal;
            }

            var count = x.Length;
            var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

            var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            var meanX = sumOfX / count;
            var meanY = sumOfY / count;
            var dblR = rNumerator / Sqrt(rDenom);

            rSquared = dblR * dblR;
            yIntercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;

        }

        public (double, double) NetLinear(decimal[] x, decimal[] y, decimal risk)
        {
            var _x = x.Select(el => double.Parse(el.ToString())).ToArray();
            var _y = y.Select(el => double.Parse(el.ToString())).ToArray();
            var alphaBeta =  MathNet.Numerics.Fit.Line(_x, _y);

            var d = _x.Select(el => new double[] { double.Parse(el.ToString()), double.Parse(risk.ToString()) }).ToArray();
           // _x = _x.Select(el => new[] { double.Parse(el.ToString()), 3.0 }).ToArray();
            var regression = Fit.MultiDim(d,_y);

            return alphaBeta;
        }

        private  decimal Sqrt(decimal x, decimal? guess = null)
        {
            var ourGuess = guess.GetValueOrDefault(x / 2m);
            var result = x / ourGuess;
            var average = (ourGuess + result) / 2m;

            if (average == ourGuess) // This checks for the maximum precision possible with a decimal.
                return average;
            else
                return Sqrt(x, average);
        }

        #endregion
    }
}
