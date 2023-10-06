using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Ticker.Mediator.DependencyInjection;
using System.Net.Http;
using Microsoft.Extensions.Http;
using Ticker.Domain.AppSettings;
using Ticker.Mediator.Http;
using Ticker.Mediator.Http.AlphaVantage;
namespace Ticker.Mediator.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPlatformMediator(this IServiceCollection services)
        {
            services.AddSingleton<IPlatformMediator, PlatformMediator>();
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            return services;
        }

        public static IServiceCollection AddHttpClient<T,K>(this IServiceCollection services) where T: IHttpClient<K> where K: ApiConfigurationOptions
        {
            services.AddHttpClient<IAlphaVantageHttpClient, AlphaVantageHttpClient>();
            return services;
        }
    }
}
