using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.Ticker
{
    public interface ITickerCalculationDomainService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="timeSeries"></param>
        /// <returns></returns>
        public List<DatePrice> CalculateReturns(DateTime? from, DateTime? to, Dictionary<DateTime, OHLCV> timeSeries);

        /// <summary>
        /// https://firemymoneymanager.com/getting-started-using-python-to-find-alpha/
        /// https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/july/test-run-linear-regression-using-csharp#understanding-linear-regression
        /// https://www.allquant.co/post/linear-regression-finding-alpha-and-beta
        /// </summary>
        /// <param name="historicalData"></param>
        /// <param name="benchMarkData"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="riskFreeRate"></param>
        /// <returns></returns>
        public double CalculateAlpha(Dictionary<DateTime, OHLCV> historicalData, Dictionary<DateTime, OHLCV> benchMarkData, DateTime from, DateTime to, double riskFreeRate);

        public double CalculateBeta(List<DatePrice> historicalData, List<DatePrice> benchMarkData);

        public double CalculateExpectedReturn(double beta, double riskFreeRate, List<DatePrice> benchMarkData);

        public double Covariance(List<DatePrice> historicalData, List<DatePrice> benchMarkData);

        public double Variance(List<DatePrice> data);
    }
}
