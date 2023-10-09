using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.Ticker
{
    public interface ITickerCalculator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="timeSeries"></param>
        /// <returns></returns>
        public List<DatePrice> Returns(DateTime? from, DateTime? to, Dictionary<DateTime, OHLCV> timeSeries);

        /// <summary>
        /// https://firemymoneymanager.com/getting-started-using-python-to-find-alpha/
        /// https://learn.microsoft.com/en-us/archive/msdn-magazine/2015/july/test-run-linear-regression-using-csharp#understanding-linear-regression
        /// https://www.allquant.co/post/linear-regression-finding-alpha-and-beta
        /// https://numerics.mathdotnet.com/Regression
        /// https://www.newyorkfed.org/medialibrary/media/research/staff_reports/sr340.pdf
        /// </summary>
        /// <param name="historicalData"></param>
        /// <param name="benchMarkData"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="riskFreeRate"></param>
        /// <returns></returns>
        public decimal Alpha(Dictionary<DateTime, OHLCV> historicalData, Dictionary<DateTime, OHLCV> benchMarkData, DateTime? from, DateTime? to, decimal riskFreeRate);

        public decimal Beta(List<DatePrice> historicalData, List<DatePrice> benchMarkData);

        public decimal ExpectedReturn(decimal beta, decimal riskFreeRate, List<DatePrice> benchMarkData);

        public decimal Covariance(List<DatePrice> historicalData, List<DatePrice> benchMarkData);

        public void LinearRegression(decimal[] x, decimal[] y, out decimal rSquared, out decimal yIntercept, out decimal slope);

        public (double, double) NetLinear(decimal[] x, decimal[] y, decimal risk);


    }
}
