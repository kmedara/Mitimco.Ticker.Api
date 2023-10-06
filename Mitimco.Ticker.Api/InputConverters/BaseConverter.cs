using Microsoft.Azure.Functions.Worker.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Mediator.Requests.Ticker;

namespace Ticker.Api.InputConverters
{
    internal abstract class BaseConverter<T> : IInputConverter
    {

        public virtual async ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
        {
            var bindingData = context.FunctionContext.BindingContext.BindingData;
            var parameters = ConvertBindingData(bindingData!);
            BaseConverter<T>.Validate(parameters);
            return ConversionResult.Success(parameters);
        }

        public virtual async ValueTask<ConversionResult> ConvertAsync<TInput>(ConverterContext context, TInput parameters)
        {
            BaseConverter<TInput>.Validate(parameters);
            return ConversionResult.Success(parameters);
        }



        private static void Validate(T obj)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj!, new ValidationContext(obj!, null, null), validationResults, true);

            if (!isValid)
            {
                var exception = new Exception();
                foreach (var validationResult in validationResults)
                {
                    exception.Data.Add(validationResult.MemberNames.FirstOrDefault() ?? "", validationResult.ErrorMessage);
                }
                throw exception;
            }


        }

        protected static T ConvertBindingData(IReadOnlyDictionary<string, object> bindingData)
        {
            var data = Activator.CreateInstance<T>();
            var type = data!.GetType();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            foreach (var item in bindingData)
            {
                var property = type.GetProperties().Where(el => string.Equals(el.Name, item.Key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (property == null) { continue; }

                Type t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                property?.SetValue(data, Convert.ChangeType(item.Value, t));
            }
            return data!;
        }

        /// <summary>
        /// Sets default time range to year to date if both values are null
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="parameters"></param>
        /// <param name="fromDateKey"></param>
        /// <param name="toDateKey"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        internal TInput CheckDates<TInput>(TInput parameters, string fromDateKey, string toDateKey)
        {
            var type = parameters?.GetType();

            var from = type?.GetProperty(fromDateKey) ?? throw new Exception($"{nameof(fromDateKey)} not found on type {typeof(T).Name}");
            var to = type?.GetProperty(toDateKey) ?? throw new Exception($"{nameof(toDateKey)} not found on type {typeof(T).Name}");

            if(from.GetValue(parameters) == null && to.GetValue(parameters) == null) { 
                from.SetValue(parameters, new DateTime(DateTime.Now.Year, 1,1));
                to.SetValue(parameters, DateTime.Today);
            
            }

            return parameters;
        }
    }
}
