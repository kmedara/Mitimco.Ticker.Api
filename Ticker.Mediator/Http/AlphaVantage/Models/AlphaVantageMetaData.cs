using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Mediator.Http.AlphaVantage.Models
{
    public record AlphaVantageMetaData
    {
        [JsonProperty("1. Information")]
        public string Information { get; set; } = string.Empty;

        [JsonProperty("1. Symbol")]
        public string Symbol { get; set; } = string.Empty;
        public string LastRefreshed { get; set; } = string.Empty;

        [JsonProperty("1. Output Size")]
        public string OutputSize { get; set; } = string.Empty;

        public string TimeZone { get; set; } = string.Empty;
    }
}
