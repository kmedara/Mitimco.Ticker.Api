using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.AppSettings
{
    /// <summary>
    /// Configuration Options for individual API clients
    /// </summary>
    public abstract record ApiConfigurationOptions
    {
        public required string BaseUrl { get; set; }
        public required ApiKey ApiKey { get; set; }
    }
}
