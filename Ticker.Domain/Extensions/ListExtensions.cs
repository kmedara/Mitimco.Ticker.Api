using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.Ticker;

namespace Ticker.Domain.Extensions
{
    public static class ListExtensions
    {
        public static decimal Average(this List<DatePrice> list)
        {
            return list.Select(el => decimal.Parse(el.Value)).Average();
        }
    }
}
