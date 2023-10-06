using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker.Api.Extensions
{
    /// <summary>
    /// Exception Handling middleware
    /// https://github.com/Azure/azure-functions-dotnet-worker/issues/414
    /// </summary>
    internal static class FunctionContextExtensions
    {
        internal static HttpRequestData? GetHttpRequestData(this FunctionContext context)
        {
            var keyValuePair = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
            var functionBindingsFeature = keyValuePair.Value;
            var type = functionBindingsFeature.GetType();
            var inputData = type.GetProperties().Single(p => p.Name == "InputData").GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
            return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
        }

        internal static void InvokeResult(this FunctionContext context, HttpResponseData response)
        {
            var keyValuePair = context.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
            var functionBindingsFeature = keyValuePair.Value;
            var type = functionBindingsFeature.GetType();
            var result = type.GetProperties().Single(p => p.Name == "InvocationResult");
            result.SetValue(functionBindingsFeature, response);
        }
    }
}
