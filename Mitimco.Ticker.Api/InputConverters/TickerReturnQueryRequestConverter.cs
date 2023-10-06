using Microsoft.Azure.Functions.Worker.Converters;
using Ticker.Mediator.Requests.Ticker;
namespace Ticker.Api.InputConverters
{
    /// <summary>
    /// https://techiesweb.net/2023/02/11/azure-functions-input-converters.html
    /// </summary>
    internal class TickerReturnQueryRequestConverter : BaseConverter<TickerReturnQueryRequest>
    {
        public override ValueTask<ConversionResult> ConvertAsync(ConverterContext context)
        {
            var bindingData = context.FunctionContext.BindingContext.BindingData;
            var parameters = ConvertBindingData(bindingData!);

            parameters = base.CheckDates(parameters, nameof(TickerAlphaQueryRequest.From), nameof(TickerAlphaQueryRequest.To));



            return base.ConvertAsync(context, parameters);
        }
    }
}
