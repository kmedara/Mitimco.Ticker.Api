using Newtonsoft.Json;
using Ticker.Domain.Ticker;

namespace Ticker.Mediator.Http.AlphaVantage.Models
{
    /// <summary>
    /// TODO: Add custom json converter for OHLCV data in domain project (do not add as an attribute, it will be used on code during manual deserialization)
    /// </summary>
    public struct AlphaVantageOHLCV
    {
        [JsonProperty("1. open")]
        public string Open { get; set; }

        [JsonProperty("2. high")]

        public string High { get; set; }
        [JsonProperty("3. low")]

        public string Low { get; set; }

        [JsonProperty("4. close")]

        public string Close { get; set; }

        [JsonProperty("5. Volume")]

        public string Volume { get; set; }
    }
}