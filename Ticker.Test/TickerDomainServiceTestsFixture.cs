//using Ticker.Domain.Ticker;

//namespace Ticker.Test
//{
//    public class TickerDomainServiceTestsFixture: IDisposable
//    {
//        public readonly TickerCalculationDomainService _domainService = new();

//        public List<DatePrice> realDailyReturns { get; private set; }

//        /// <summary>
//        /// from, to, ticker, expected alpha
//        /// </summary>
//        public List<(DateTime from, DateTime to, double alpha, double riskFreeRate)> tickerAlphas { get; private set; }

//        public TickerDomainServiceTestsFixture() {
        
//            realDailyReturns = new List<DatePrice> {
//                new (new DateTime(2023,10,5), .72),
//                new (new DateTime(2023,10,4), .73)

//            };

//            tickerAlphas = new()
//        {
//            new (new DateTime(2023,10,1), new DateTime(2023,10,6), 4, (4.38 / 100))
//        };

//        }
        

//        public void Dispose()
//        {
//        }
//    }
//}