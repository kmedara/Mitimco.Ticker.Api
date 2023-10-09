using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Ticker.Validation
{

    public enum Range
    {
        YEARS,
        MONTHS, DAYS, HOURS,
        MINUTES, SECONDS
    }
    /// <summary>
    /// Use a negative number tto set limit in the past
    /// 
    /// <para>(-5,YEARS) = 5 years ago</para>
    /// <para>(5, YEARS) = in 5 years</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LimitDateToWithinAttribute : ValidationAttribute
    {
        private readonly int _length;
        private readonly Range _range;
        public LimitDateToWithinAttribute(int length, Range range)
        {

            _length = length;
            _range = range;
        }
        public override bool IsValid(object? value)
        {
            if (value == null) return true;

            var isNegative = _length > 0;
            var operation = isNegative ? LessThanOrEqualTo : GreaterThanOrEqualTo;
            var today = DateTime.Today;
            var limit = new DateTime();
            switch (_range)
            {
                case Range.YEARS:
                    limit = today.AddYears(_length);
                    break;
                case Range.MONTHS:
                    limit = today.AddMonths(_length);
                    break;
                case Range.DAYS:
                    limit = today.AddDays(_length);
                    break;
                default:
                    throw new ArgumentException(nameof(_range));
            }

            ErrorMessage ??= GetErrorVerbiage(limit, isNegative);
            return operation((DateTime)value!, limit);
        }

        private string GetErrorVerbiage(DateTime limit, bool isNegative)
        {
            ///matches all capital letters, replaces them with a space and the letter we found ($1), then trims the result to remove the initial space if there was a capital letter at the beginning.
            var name = System.Text.RegularExpressions.Regex.Replace(
                isNegative ? nameof(LessThanOrEqualTo) : nameof(GreaterThanOrEqualTo),
                "([A-Z])",
                " $1",
                System.Text.RegularExpressions.RegexOptions.Compiled).Trim();

            ///operation.Method.Name was returning Func`3 for whatever reason
            return $"Limit set to { _length } { _range.ToString().ToLower() } Date Must be {name.ToLower()} {limit.ToShortDateString()}";

        }

        private  Func<DateTime, DateTime, bool> GreaterThanOrEqualTo = (DateTime x, DateTime y) => x >= y;
        private  Func<DateTime, DateTime, bool> LessThanOrEqualTo = (DateTime x, DateTime y) => x <= y;

    }

}
