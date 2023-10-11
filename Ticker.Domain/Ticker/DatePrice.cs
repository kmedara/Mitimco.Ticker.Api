using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Domain.Ticker
{
    /// <summary>
    /// simple poco denoting a price related to a date, could be anything 
    /// </summary>
    public struct DatePrice
    {
        public DatePrice(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; set; }
        public decimal Value {  get; set; }
    }
}
