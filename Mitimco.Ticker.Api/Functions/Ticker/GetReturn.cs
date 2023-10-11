using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ticker.Api.InputBinding.Converters;
using Ticker.Api.Utility;
using Ticker.Domain.AppSettings;
using Ticker.Mediator.DependencyInjection;
using Ticker.Mediator.Requests.Ticker;
namespace Ticker.Api.Functions.Ticker
{
    public class GetReturn
    {
        private readonly ILogger _logger;
        private readonly IPlatformMediator _mediator;

        public GetReturn(ILoggerFactory loggerFactory, IPlatformMediator mediator)
        {
            _mediator = mediator;
            _logger = loggerFactory.CreateLogger<GetReturn>();
        }

        [Function(nameof(GetReturn))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.Ticker.Returns.Get)] HttpRequestData req,
            [InputConverter(typeof(TickerReturnQueryRequestConverter))] TickerReturnQueryRequest request)
        {
            var result = await _mediator.Send(request);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
