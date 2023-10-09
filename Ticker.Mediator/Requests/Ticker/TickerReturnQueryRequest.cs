
using MediatR;
using System.ComponentModel.DataAnnotations;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage;
using Ticker.Validation;
namespace Ticker.Mediator.Requests.Ticker
{
    [YearToDateDefaultRange(nameof(From), nameof(To))]
    public record TickerReturnQueryRequest : IRequest<TickerReturnQueryResponse>
    {
        [Required]
        public required string Ticker { get; set; }

        [LimitDateToWithin(-5, Validation.Range.YEARS)]
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
        private readonly ITickerCalculator _service;

        public TickerReturnQueryRequestHandler(IAlphaVantageHttpClient client, ITickerCalculator service)
        {
            _client = client;
            _service = service;
        }
        public async Task<TickerReturnQueryResponse> Handle(TickerReturnQueryRequest request, CancellationToken cancellationToken)
        {
            var clientResponse = await _client.GetDailyOHLCV(request.Ticker);
            var returns = _service.Returns(request.From, request.To, clientResponse.TimeSeriesDaily.ToDictionary(item => DateTime.Parse(item.Key), item => new OHLCV { Close = item.Value.Close }));

            return new TickerReturnQueryResponse(returns);
        }
    }


}
