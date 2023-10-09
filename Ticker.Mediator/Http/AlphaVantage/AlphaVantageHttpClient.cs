using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.AppSettings;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Http.AlphaVantage.Responses;
using Ticker.Mediator.Requests.Ticker;

namespace Ticker.Mediator.Http.AlphaVantage
{
    public sealed class AlphaVantageHttpClient : BaseHttpClient<AlphaApiConfigurationOptions>, IAlphaVantageHttpClient
    {
        public AlphaVantageHttpClient(HttpClient client, AlphaApiConfigurationOptions options) : base(client, options) { }

        public async Task<CompanyOverviewResponse> GetCompanyOverview(string ticker)
        {
            var query = buildQueryString(new Dictionary<string, string> {
                { _options.ApiKey.Identifier, _options.ApiKey.Value },
                { "function", "OVERVIEW" },
                { "symbol", "ticker" },
                { _options.ApiKey.Identifier, _options.ApiKey.Value }

            });
            return await GetAsync<CompanyOverviewResponse>(query);
        }

        /// <summary>
        /// Gets 10-year treasury bond as benchmark
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TreasuryYieldResponse> GetCurrentRiskFreeRate()
        {
            var query = buildQueryString(new Dictionary<string, string> {
                { _options.ApiKey.Identifier, _options.ApiKey.Value },
                { "function", "TREASURY_YIELD" },
                { "maturity", "3month" },
                { "interval", "monthly" }

            });
            return await GetAsync<TreasuryYieldResponse>(query);
        }

        /// <summary>
        /// https://www.alphavantage.co/documentation/#daily
        /// </summary>
        /// <typeparam name="TickerReturnQueryResponse"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<AlphaVantageTimeSeriesDailyResponse> GetDailyOHLCV(string ticker)
        {

            var query = buildQueryString(new Dictionary<string, string> {
                { _options.ApiKey.Identifier, _options.ApiKey.Value },
                { "function", "TIME_SERIES_DAILY" },
                { "symbol", ticker },
                { "outputsize", "full" }

            });
            return await GetAsync<AlphaVantageTimeSeriesDailyResponse>(query);

        }

        protected override async Task<T> GetAsync<T>(string query)
        {
            var response = await base.GetAsync<T>("query?" + query);
            return response;
        }
    }
}
