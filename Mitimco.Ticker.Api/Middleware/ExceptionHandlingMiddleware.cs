using Grpc.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Api.Middleware
{
    /// <summary>
    /// Single Application Exception handler 
    /// //TODO: More robust error response based on Problem Details rfc 7807
    /// //TODO: Implement mapping between exceptions and status codes
    /// this is a great library for problem details to port over to function apps https://github.com/khellang/Middleware/tree/master
    /// </summary>

    internal class ExceptionHandlingMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);

            }
            catch (Exception ex)
            {
                var request = await context.GetHttpRequestDataAsync();

                var details = CreateProblemDetails(ex, context);



                var response = request!.CreateResponse();

                await response.WriteAsJsonAsync(details);
                response.StatusCode = ((HttpStatusCode)details.Status!);

                context.GetInvocationResult().Value = response;
            }
        }

        private static ProblemDetails CreateProblemDetails(Exception ex, FunctionContext context)
        {
            var details = new StatusCodeProblemDetails(500);

            foreach (DictionaryEntry data in ex.Data)
            {
                details.Extensions.Add(data.Key.ToString() ?? "", data.Value?.ToString());
            }
            details.Extensions.Add("InvocationId", context.InvocationId);
            details.Instance = context.FunctionDefinition.Name;
            details.Detail = ex.InnerException?.Message ?? ex.Message;
            return details;
        }
    }
}
