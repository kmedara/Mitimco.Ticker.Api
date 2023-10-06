using Microsoft.Azure.Functions.Worker.Converters;
using Ticker.Mediator.Requests.Ticker;
namespace Ticker.Api.InputConverters
{
    internal class TickerAlphaQueryRequestConverter : BaseConverter<TickerAlphaQueryRequest>
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
