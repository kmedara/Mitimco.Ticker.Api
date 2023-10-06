using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage.Models;

namespace Ticker.Mediator.Http.AlphaVantage.Responses
{

    public struct AlphaVantageTimeSeriesDailyResponse
    {
        [JsonProperty("Meta Data")]
        public AlphaVantageMetaData MetaData { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<string, AlphaVantageOHLCV> TimeSeriesDaily { get; set; }
    }
}
