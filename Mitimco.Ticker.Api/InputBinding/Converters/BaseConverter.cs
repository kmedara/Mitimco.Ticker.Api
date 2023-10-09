using Microsoft.Azure.Functions.Worker.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Mediator.Requests.Ticker;

namespace Ticker.Api.InputBinding.Converters
{
    internal abstract class BaseConverter<T> : IValidationConverter<T>
    {
        public IValidationConverter<T> AsIValidationConverter() => this;

        public virtual async ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
        {
            var bindingData = context.FunctionContext.BindingContext.BindingData;
            var parameters = AsIValidationConverter().ConvertBindingData(bindingData!);
            AsIValidationConverter().Validate(parameters);
            return ConversionResult.Success(parameters);
        }

        public virtual async ValueTask<ConversionResult> ConvertAsync(ConverterContext context, T parameters)
        {
            AsIValidationConverter().Validate(parameters);
            return ConversionResult.Success(parameters);
        }
    }
}
