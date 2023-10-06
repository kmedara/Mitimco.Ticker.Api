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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ticker.Mediator.Requests.Ticker
{
    public record TickerAlphaQueryRequest : IRequest<double>
    {
        [Required]
        public required string Ticker { get; set; }

        [Required]
        public required string BenchmarkTicker { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    internal class TickerAlphaQueryRequestHandler : IRequestHandler<TickerAlphaQueryRequest, double>
    {
        private readonly IAlphaVantageHttpClient _client;
        private readonly ITickerCalculationDomainService _service;

        public TickerAlphaQueryRequestHandler(IAlphaVantageHttpClient client, ITickerCalculationDomainService service)
        {
            _client = client;
            _service = service;
        }
        public async Task<double> Handle(TickerAlphaQueryRequest request, CancellationToken cancellationToken)
        {
            var stockDataPoints = await _client.GetDailyOHLCV(request.Ticker);
            var benchMarkDataPoints = await _client.GetDailyOHLCV(request.BenchmarkTicker);
            var risk = (await _client.GetCurrentRiskFreeRate()).data.FirstOrDefault()!.Value;
            var riskFreeRate = risk / 100;

            var stockPrices = stockDataPoints.TimeSeriesDaily.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close });
            var benchmarkPrices = benchMarkDataPoints.TimeSeriesDaily.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close });


            var lg = new LinearRegression(stockDataPoints.TimeSeriesDaily.Select(el => Double.Parse(el.Value.Close)).ToArray(), benchMarkDataPoints.TimeSeriesDaily.Select(el => Double.Parse(el.Value.Close)).ToArray());
            lg.Plot();


            var alpha = _service.CalculateAlpha(
               stockPrices,
               benchmarkPrices,
               request.From!.Value,
                request.To!.Value,
                riskFreeRate);

            return alpha;
        }
    }
}
