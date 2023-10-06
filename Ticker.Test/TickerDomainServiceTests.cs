//using Newtonsoft.Json.Linq;
//using System.Collections.Generic;
//using Ticker.Domain.Ticker;
//using Ticker.Mediator.Http.AlphaVantage;
//using Ticker.Mediator.Http.AlphaVantage.Models;
//using Ticker.Test.Utility;
//using Xunit;

//namespace Ticker.Test
//{
//    public class TickerDomainServiceTests : IClassFixture<TickerDomainServiceTestsFixture>
//    {
//        private readonly TickerDomainServiceTestsFixture _fixture;

//        public TickerDomainServiceTests(TickerDomainServiceTestsFixture fixture)
//        {
//            _fixture = fixture!;
//        }



//        /// <summary>
//        /// Yields true historical data for date, along with data to test against
//        /// </summary>
//        /// <returns></returns>
//        public static IEnumerable<object[]> HistoricalOHLCVData()
//        {
//            //var timeSeries = FileUtility.GetHistoricalData();
//            //var x = _fixture.realDailyReturns;
//            //for (int i = 0; i < .Count; i++)
//            //{
//            //    DatePrice date = _fixture.realDailyReturns[i];
//            //    yield return new object[] { date.Date, date.Return, timeSeries!.Take(10) };
//            //}
//        }

//        //public static IEnumerable<object[]> BenchmarkOHLCVData()
//        //{
//        //    var timeSeries = FileUtility.GetHistoricalData();

//        //    //foreach (var date in realDailyReturns)
//        //    //{
//        //    //    yield return new object[] { date.Date, date.Return, timeSeries!.Take(10) };
//        //    //}
//        //}

//        public static IEnumerable<object[]> BenchmarkAndHistoricalOHLCVData()
//        {
//            var stockSeries = FileUtility.GetHistoricalData();

//            var benchMarkSeries = FileUtility.GetBenchmarkData();

//            //foreach (var data in tickerAlphas)
//            //{
//            //    yield return new object[] { data.from, data.to, data.alpha, stockSeries, benchMarkSeries, data.riskFreeRate };
//            //}
//        }



//        public TickerDomainServiceTests()
//        {
//        }

//        [Theory]
//        [MemberData(nameof(HistoricalOHLCVData))]
//        public void DailyReturn_Should_Be_Correct(DateTime date, double expectedReturnPercent, Dictionary<DateTime, OHLCV> series)
//        {
//            //var result = _domainService.CalculateDailyReturns(null, null, series.ToDictionary(item => item.Key, item => item.Value));

//            var returnForDate = result.Find(el => el.Date == date);

//            var t = Math.Round(returnForDate.Return * 100, 2, MidpointRounding.AwayFromZero);

//            Assert.Equal(expectedReturnPercent, t);
//        }


//        [Theory]
//        [MemberData(nameof(BenchmarkAndHistoricalOHLCVData))]
//        public void Alpha_Should_Be_Correct(DateTime from, DateTime to, double expectedAlphaPercent, Dictionary<DateTime, OHLCV> stockSeries, Dictionary<DateTime, OHLCV> benchMarkSeries, double riskFreeRate)
//        {
//            //var result = _domainService.CalculateAlpha(stockSeries, benchMarkSeries, from, to, riskFreeRate);

//            //Assert.Equal(expectedAlphaPercent, result * 100);
//        }
//    }
//}
