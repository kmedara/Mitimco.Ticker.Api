using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.AppSettings;

namespace Ticker.Mediator.Http
{
    public abstract class BaseHttpClient<T>: IHttpClient<T> where T:ApiConfigurationOptions
    {
        protected HttpClient _client;
        protected T _options; 
        protected BaseHttpClient(HttpClient client, T options)
        {
            _options = string.IsNullOrEmpty(options?.BaseUrl)? throw new ArgumentNullException(nameof(options)): options ;
            _client = client;

            _client.BaseAddress = new Uri(_options.BaseUrl);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected BaseHttpClient<T> AddApiKeyHeader()
        {
            _client.DefaultRequestHeaders.Add(_options.ApiKey.Identifier, _options.ApiKey.Value);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="uri"></param>
        /// <returns></returns>
        protected virtual async Task<TOutput> GetAsync<TOutput>(string uri)
        {
            var content = await GetContent(_client.GetAsync(uri));
            return JsonConvert.DeserializeObject<TOutput>(content)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="uri"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        protected async Task<TOutput> GetAsync<TOutput>(string uri, JsonConverter converter)
        {
            var content = await GetContent(_client.GetAsync(uri));
            return JsonConvert.DeserializeObject<TOutput>(content, converter)!;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private static async Task<string> GetContent(Task<HttpResponseMessage> task)
        {
            var response = await task;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }

        /// <summary>
        /// Build query string from dictionary
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal string buildQueryString(Dictionary<string,string> parameters)
        {
            StringBuilder sb = new StringBuilder();

            for(var i  = 0; i < parameters.Count; i++)
            {
                var item = parameters.ElementAt(i);
                sb.Append(item.Key);
                sb.Append('=');
                sb.Append(item.Value);
                sb.Append('&');
            }
            return sb.ToString();
        }
    }
}
