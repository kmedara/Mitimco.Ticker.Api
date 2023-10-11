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
        public decimal Open { get; set; }
        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal Volume { get; set; }
    }
}
