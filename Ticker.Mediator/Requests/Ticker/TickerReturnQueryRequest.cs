
using MediatR;
using System.ComponentModel.DataAnnotations;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage;
using Ticker.Validation;
namespace Ticker.Mediator.Requests.Ticker
{
    [YearToDateDefaultRange(nameof(From), nameof(To))]
    public record TickerReturnQueryRequest : IRequest<IEnumerable<DatePrice>>
    {
        [Required]
        public required string Ticker { get; set; }

        [LimitDateToWithin(-5, Validation.Range.YEARS)]
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }

    internal class TickerReturnQueryRequestHandler : IRequestHandler<TickerReturnQueryRequest, IEnumerable<DatePrice>>
    {
        private readonly IAlphaVantageHttpClient _client;
        private readonly ITickerCalculator _service;

        public TickerReturnQueryRequestHandler(IAlphaVantageHttpClient client, ITickerCalculator service)
        {
            _client = client;
            _service = service;
        }
        public async Task<IEnumerable<DatePrice>> Handle(TickerReturnQueryRequest request, CancellationToken cancellationToken)
        {
            var clientResponse = await _client.GetDailyOHLCV(request.Ticker);

            var data = clientResponse.TimeSeriesDaily?.Select(el => el.Value.Close).ToArray() ?? throw new Exception($"Unable to find data for specified ticker: { request.Ticker }");

            var returns = _service.Returns(data);

            return returns.Select((e, i) => new DatePrice(clientResponse.TimeSeriesDaily.ElementAt(i).Key, e));
        }
    }


}
