using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Mediator.Http.AlphaVantage.Models
{
    public record CompanyOverview
    {
        [JsonProperty("Symbol")]
        public string Ticker { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Exchange {  get; set; } = string.Empty;

    }
}
