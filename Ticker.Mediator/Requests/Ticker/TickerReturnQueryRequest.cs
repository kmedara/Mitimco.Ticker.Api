
using MediatR;
using System.ComponentModel.DataAnnotations;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage;

namespace Ticker.Mediator.Requests.Ticker
{
    public record TickerReturnQueryRequest : IRequest<TickerReturnQueryResponse>
    {
        [Required]
        public required string Ticker { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    public class TickerReturnQueryResponse : List<DatePrice>
    {
        public TickerReturnQueryResponse(List<DatePrice> list) : base(list) { }
    }

    internal class TickerReturnQueryRequestHandler : IRequestHandler<TickerReturnQueryRequest, TickerReturnQueryResponse>
    {
        private readonly IAlphaVantageHttpClient _client;
        private readonly ITickerCalculationDomainService _service;

        public TickerReturnQueryRequestHandler(IAlphaVantageHttpClient client, ITickerCalculationDomainService service)
        {
            _client = client;
            _service = service;
        }
        public async Task<TickerReturnQueryResponse> Handle(TickerReturnQueryRequest request, CancellationToken cancellationToken)
        {
            var clientResponse = await _client.GetDailyOHLCV(request.Ticker);

            var dailies = _service.CalculateReturns(request.From,request.To, clientResponse.TimeSeriesDaily.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close }));

            return new TickerReturnQueryResponse(dailies);
        }
    }


}
