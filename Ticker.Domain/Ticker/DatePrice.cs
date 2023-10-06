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
    public class DatePrice
    {
        public DatePrice(DateTime date, double value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; set; }
        public double Value {  get; set; }
    }
}
