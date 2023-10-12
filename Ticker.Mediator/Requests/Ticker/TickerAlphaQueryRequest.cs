using MathNet.Numerics;
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
using Ticker.Domain.Ticker.AlphaCalculationStrategy;
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

        public DateTime? From { get; set; }
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
            var risks = await _client.GetCurrentRiskFreeRate();
            var risk = risks.Data.Select(el => el.Value).First() / 100;

            var stockPrices = stockDataPoints.TimeSeriesDaily?
                .Where(el =>  request.From == null || el.Key >= request.From)
                .Where(el => el.Key <= request.To || request.To == null)
                .OrderBy(el => el.Key)
                .Select(el => el.Value.Close)
                .ToArray();

            var benchmarkPrices = benchMarkDataPoints.TimeSeriesDaily?
                .Where(el => request.From == null || el.Key >= request.From.Value)
                .Where(el => el.Key <= request.To || request.To == null)
                .OrderBy(el => el.Key)
                .Select(el => el.Value.Close)
                .ToArray();

            _calculator.SetAlphaStrategy(new CAPMFormulaStrategy());

            var hReturns = _calculator.Returns(stockPrices);
            var bReturns = _calculator.Returns(benchmarkPrices);


            var x = Fit.Line(stockPrices.Select(el => (double)el).ToArray(), benchmarkPrices.Select(el => (double)el).ToArray());

            var alpha = _calculator.Alpha(
               stockPrices,
               benchmarkPrices,
                risk);

            return alpha;
        }
    }
}
