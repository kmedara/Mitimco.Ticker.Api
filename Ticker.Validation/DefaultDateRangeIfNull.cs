using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Validation
{

    /// <summary>
    /// Used to set default values for date range keys if both are set to null
    /// kind of a hack and only works if validation is run on an object, but it works well for DTOs and requests in controllers
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class YearToDateDefaultRange : ValidationAttribute
    {
        private readonly string _fromKey;
        private readonly string _toKey;

        public YearToDateDefaultRange(
            string fromKey,
            string toKey)
        {
            _fromKey = fromKey;
            _toKey = toKey;

        }
        public override bool IsValid(object? value)
        {
            var type = value?.GetType();

            var from = type?.GetProperty(_fromKey) ?? throw new Exception($"{nameof(_fromKey)} not found on type {type?.Name}");
            var to = type?.GetProperty(_toKey) ?? throw new Exception($"{nameof(_toKey)} not found on type {type?.Name}");

            if (from.GetValue(value) == null && to.GetValue(value) == null)
            {
                from.SetValue(value, new DateTime(DateTime.Now.Year, 1, 1));
                to.SetValue(value, DateTime.Today);
            }
            return true;

        }
    }
}


//var type = parameters?.GetType();

//var from = type?.GetProperty(fromDateKey) ?? throw new Exception($"{nameof(fromDateKey)} not found on type {typeof(T).Name}");
//var to = type?.GetProperty(toDateKey) ?? throw new Exception($"{nameof(toDateKey)} not found on type {typeof(T).Name}");

//if (from.GetValue(parameters) == null && to.GetValue(parameters) == null)
//{
//    from.SetValue(parameters, new DateTime(DateTime.Now.Year, 1, 1));
//    to.SetValue(parameters, DateTime.Today);

//}

//if (from.GetValue(parameters) != null && to.GetValue(parameters) == null)
//    to.SetValue(parameters, DateTime.Today);

//if (from.GetValue(parameters) == null && to.GetValue(parameters) != null)
//    from.SetValue(parameters, DateTime.MinValue);


//return parameters;