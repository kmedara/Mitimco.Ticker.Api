//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using Ticker.Domain.AppSettings;
//using Ticker.Mediator.Http;
//using Ticker.Mediator.Http.AlphaVantage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ticker.Domain.AppSettings;
using Ticker.Mediator.Http;

namespace Ticker.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Binds Config of name T to options of type T
        /// 
        /// https://andrewlock.net/reloading-strongly-typed-options-in-asp-net-core-1-1-0/#if-you-don-t-like-to-use-ioptions
        /// then 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services) where T : class
        {

            
            services.ConfigureOptions<T>(typeof(T).Name);
            return services;

        }

        /// <summary>
        /// Use if section name is nested
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureOptions<T>(this IServiceCollection services, string sectionName) where T : class
        {

            services.AddOptions<T>().Configure<IConfiguration>((settings, configuration) => {
                configuration.GetSection(sectionName).Bind(settings);
            });

            services.AddSingleton(cfg => cfg.GetService<IOptions<T>>()!.Value);

            return services;

        }


        /// <summary>
        /// Configures an HttpClient and its options for dependency injection
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientWithOptions<TInterface, TImplementation, TOptions>(this IServiceCollection services) 
            where TInterface : class, IHttpClient<TOptions>
            where TImplementation: class, TInterface
            where TOptions : ApiConfigurationOptions

        {
            services.ConfigureOptions<TOptions>();
            services.AddHttpClient<TInterface, TImplementation>();
            return services;
        }

        /// <summary>
        /// Adds HttpClient with api options for dependency injection
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TOptions"></typeparam>
        /// <param name="services"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpClientWithOptions<TInterface, TImplementation, TOptions>(this IServiceCollection services, string sectionName)
            where TInterface : class, IHttpClient<TOptions>
            where TImplementation : class, TInterface
            where TOptions : ApiConfigurationOptions

        {
            services.ConfigureOptions<TOptions>(sectionName);
            services.AddHttpClient<TInterface, TImplementation>();
            return services;
        }
    }
}
