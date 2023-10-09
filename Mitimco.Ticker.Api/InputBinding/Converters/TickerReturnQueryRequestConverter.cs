using Microsoft.Azure.Functions.Worker.Converters;
using Ticker.Mediator.Requests.Ticker;
namespace Ticker.Api.InputBinding.Converters
{
    /// <summary>
    /// https://techiesweb.net/2023/02/11/azure-functions-input-converters.html
    /// </summary>
    internal class TickerReturnQueryRequestConverter : BaseConverter<TickerReturnQueryRequest>
    {
    }
}
