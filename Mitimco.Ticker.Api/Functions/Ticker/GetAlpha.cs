using Microsoft.Azure.Functions.Worker.Converters;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ticker.Api.InputConverters;
using Ticker.Api.Utility;
using Ticker.Mediator.DependencyInjection;
using Ticker.Mediator.Requests.Ticker;

namespace Ticker.Api.Functions.Ticker
{
    internal class GetAlpha
    {
        private readonly ILogger _logger;
        private readonly IPlatformMediator _mediator;

        public GetAlpha(ILoggerFactory loggerFactory, IPlatformMediator mediator)
        {
            _mediator = mediator;
            _logger = loggerFactory.CreateLogger<GetReturn>();
        }

        [Function(nameof(GetAlpha))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = Routes.Ticker.Alpha.Get)] HttpRequestData req,
            [InputConverter(typeof(TickerAlphaQueryRequestConverter))] TickerAlphaQueryRequest request)
        {
            var result = await _mediator.Send(request);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);
            return response;
        }
    }
}
