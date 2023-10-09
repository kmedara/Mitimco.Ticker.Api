using Ticker.Domain.Ticker;

namespace Ticker.Mediator.Http.AlphaVantage.Responses
{
    public record TreasuryYieldResponse
    {

        public string Name { get; set; } = string.Empty;

        public string Interval {  get; set; } = string.Empty;

        public string Unit { get; set; } = string.Empty;

        public List<DatePrice> Data { get; set; } = new List<DatePrice> { };
    }
}