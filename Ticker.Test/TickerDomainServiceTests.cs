using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage;
using Ticker.Mediator.Http.AlphaVantage.Models;
using Ticker.Test.Utility;
using Xunit;

namespace Ticker.Test
{
    public class TickerDomainServiceTests
    {

        private readonly TickerCalculator _domainService = new();

        /// <summary>
        /// date, expected daily return for given date
        /// </summary>
        private static readonly List<DatePrice> realDailyReturns = new List<DatePrice> {
                new(new DateTime(2023,10,5), ".72"),
                new(new DateTime(2023,10,4), ".73")

            };

        /// <summary>
        /// from, to, ticker, expected alpha
        /// </summary>
        private static readonly List<(DateTime from, DateTime to, decimal alpha, decimal riskFreeRate)> realTickerAlphas = new()
        {
            new (new DateTime(2023,10,1), new DateTime(2023,10,5), 4, (4.38m / 100))
        };


        /// <summary>
        /// Yields true historical data for date, along with data to test against
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<object[]> HistoricalOHLCVData()
        {
            var timeSeries = FileUtility.GetHistoricalData();

            foreach (var date in realDailyReturns)
            {
                yield return new object[] { date.Date, date.Value, timeSeries };
            }
        }

        public static IEnumerable<object[]> BenchmarkOHLCVData()
        {
            var timeSeries = FileUtility.GetHistoricalData();

            foreach (var date in realDailyReturns)
            {
                yield return new object[] { date.Date, date.Value, timeSeries!.Take(10) };
            }
        }

        public static IEnumerable<object[]> BenchmarkAndHistoricalOHLCVData()
        {
            var stockSeries = FileUtility.GetHistoricalData();

            var benchMarkSeries = FileUtility.GetBenchmarkData();

            foreach (var data in realTickerAlphas)
            {
                yield return new object[] {data.from, data.to,data.alpha, stockSeries, benchMarkSeries, data.riskFreeRate };
            }
        }



        public TickerDomainServiceTests()
        {
        }

        [Theory]
        [MemberData(nameof(HistoricalOHLCVData))]
        public void DailyReturn_Should_Be_Correct(DateTime date, decimal expectedReturnPercent, Dictionary<DateTime, OHLCV> series)
        {
            var result = _domainService.Returns(null, null, series.ToDictionary(item => item.Key, item => item.Value));

            var returnForDate = result.Find(el => el.Date == date);

            var t = Math.Round(decimal.Parse(returnForDate!.Value) * 100, 2, MidpointRounding.AwayFromZero);

            Assert.Equal(expectedReturnPercent, t);
        }


        //[Theory]
        //[MemberData(nameof(BenchmarkAndHistoricalOHLCVData))]
        //public void Alpha_Should_Be_Correct(DateTime from, DateTime to, decimal expectedAlphaPercent, Dictionary<DateTime, OHLCV> stockSeries, Dictionary<DateTime, OHLCV> benchMarkSeries, decimal riskFreeRate)
        //{
        //    //var result = _domainService.Alpha(stockSeries,benchMarkSeries,from,to, riskFreeRate);

        //    //Assert.Equal(expectedAlphaPercent, result * 100);
        //    //var result = _domainService.CalculateDailyReturns(null, null, series.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV  { Close = item.Value.Close }));

        //    //var returnForDate = result.Find(el => el.Date == date);
        //    //Assert.Contains(dailyReturnPercent, returnForDate.Return);
        //}
    }
}
