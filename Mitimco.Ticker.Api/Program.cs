using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ticker.Api.Extensions;
using Ticker.Api.Middleware;
using Ticker.Domain.AppSettings;
using Ticker.Domain.Ticker;
using Ticker.Mediator.Extensions;
using Ticker.Mediator.Http.AlphaVantage;

var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults(worker =>
            {
                worker.UseMiddleware<ExceptionHandlingMiddleware>();
            })
            .ConfigureServices(s =>
            {
                s.AddHttpClientWithOptions<IAlphaVantageHttpClient, AlphaVantageHttpClient, AlphaApiConfigurationOptions>("ApiConfigurationOptions:AlphaApiConfigurationOptions")
                .AddTransient<ITickerCalculationDomainService, TickerCalculationDomainService>()
                .AddPlatformMediator();
            })
            .Build();

host.Run();
