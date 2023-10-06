using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticker.Domain.AppSettings;
using Ticker.Mediator.Http.AlphaVantage.Responses;
using Ticker.Mediator.Requests.Ticker;

namespace Ticker.Mediator.Http.AlphaVantage
{
    public interface IAlphaVantageHttpClient : IHttpClient<AlphaApiConfigurationOptions>
    {
        public Task<AlphaVantageTimeSeriesDailyResponse> GetDailyOHLCV(string ticker);

        public Task<TreasuryYieldResponse> GetCurrentRiskFreeRate();

        public Task<CompanyOverviewResponse> GetCompanyOverview(string ticker);
    }
}
