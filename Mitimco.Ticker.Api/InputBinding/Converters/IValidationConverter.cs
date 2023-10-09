using Microsoft.Azure.Functions.Worker.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Api.InputBinding.Converters
{
    /// <summary>
    /// Convert and Validate Input bindings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IValidationConverter<T> : IInputConverter
    {

        public IValidationConverter<T> AsIValidationConverter() => this;
        /// <summary>
        /// Validate Object
        /// <para>Default behavior manually calls model binding rules specified on object</para>
        /// </summary>
        public virtual void Validate(T obj)
        {
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj!, new ValidationContext(obj!, null, null), validationResults, true);

            if (!isValid)
            {
                var exception = new ValidationException("One or more input validation errors occurred");
                foreach (var validationResult in validationResults)
                {
                    exception.Data.Add(validationResult.MemberNames.FirstOrDefault() ?? "", validationResult.ErrorMessage);
                }
                throw exception;
            }
        }

        /// <summary>
        /// Convert Binding data to objects
        /// <para>Default behavior checks binding data properties and compares against object properties of same name. Ignores casing</para>
        /// </summary>
        public virtual T ConvertBindingData(IReadOnlyDictionary<string, object> bindingData)
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

    }
}
