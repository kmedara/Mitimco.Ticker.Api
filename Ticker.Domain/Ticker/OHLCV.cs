using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.Ticker
{
    /// <summary>
    /// Data points for a ticker over single day
    /// </summary>
    public struct OHLCV
    {
        public string Open { get; set; }
        public string High { get; set; }

        public string Low { get; set; }

        public string Close { get; set; }

        public string Volume { get; set; }
    }
}
