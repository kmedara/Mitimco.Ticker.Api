using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage;
using Ticker.Validation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ticker.Mediator.Requests.Ticker
{
    [YearToDateDefaultRange(nameof(From), nameof(To))]
    public record TickerAlphaQueryRequest : IRequest<decimal>
    {
        [Required]
        public required string Ticker { get; set; }

        [Required]
        public required string BenchmarkTicker { get; set; }

        [Required]
        public DateTime? From { get; set; }
        [Required]
        public DateTime? To { get; set; }
    }

    internal class TickerAlphaQueryRequestHandler : IRequestHandler<TickerAlphaQueryRequest, decimal>
    {
        private readonly IAlphaVantageHttpClient _client;
        private readonly ITickerCalculator _calculator;

        public TickerAlphaQueryRequestHandler(IAlphaVantageHttpClient client, ITickerCalculator calculator)
        {
            _client = client;
            _calculator = calculator;
        }
        public async Task<decimal> Handle(TickerAlphaQueryRequest request, CancellationToken cancellationToken)
        {
            var stockDataPoints = await _client.GetDailyOHLCV(request.Ticker);
            var benchMarkDataPoints = await _client.GetDailyOHLCV(request.BenchmarkTicker);
            var risk = ((await _client.GetCurrentRiskFreeRate()).Data.Select(el => decimal.Parse(el.Value)).Average()) / 100;

            var stockPrices = stockDataPoints.TimeSeriesDaily.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close });
            var benchmarkPrices = benchMarkDataPoints.TimeSeriesDaily.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close });

            ///comes out pretty close to the jenson alpha?
            var alpha = _calculator.Alpha(
               stockPrices,
               benchmarkPrices,
               request.From,
                request.To,
                risk);


            //var priceData = stockPrices

            //    .OrderBy(item => item.Key)
            //    .Select(item => decimal.Parse(item.Value.Close)).ToArray();

            //var priceData2 = benchmarkPrices

            //    .OrderBy(item => item.Key)
            //    .Select(item => decimal.Parse(item.Value.Close)).ToArray();


            ///linear regression worked when comparing a baseline (aapl to aapl)
            //var calc =_calculator.NetLinear(priceData, priceData2, risk);

            //_calculator.LinearRegression(priceData, priceData2, out var rsqaured, out var alph, out var beta);

            return alpha;
        }
    }
}
