namespace DeepSleep.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Health;
    using DeepSleep.Web.Controllers;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public static class PingEndpointHttpExtensionMethods
    {
        /// <summary>Uses the deep sleep ping endpoint.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepPingEndpoint(
            this IServiceCollection services,
            Action<IPingEndpointConfigurationProvider> configure = null)
        {
            var configuration = new PingEndpointConfigurationProvider
            {
                Template = "ping"
            };

            if (configure != null)
            {
                configure(configuration);
            }

            services.AddTransient<PingController>();
            services.AddTransient<IDeepSleepSingleRouteRegistrationProvider, PingRouteRegistrationProvider>((p) =>
            {
                return new PingRouteRegistrationProvider(new DeepSleepRouteRegistration(
                   template: configuration?.Template ?? "ping",
                   httpMethods: new[] { "GET" },
                   controller: typeof(PingController),
                   endpoint: nameof(PingController.Ping),
                   config: new DeepSleepRequestConfiguration
                   {
                       AllowAnonymous = true,
                       CacheDirective = new ApiCacheDirectiveConfiguration
                       {
                           Cacheability = HttpCacheType.NoCache,
                           CacheLocation = HttpCacheLocation.Private,
                           ExpirationSeconds = -1
                       },
                       CrossOriginConfig = new ApiCrossOriginConfiguration
                       {
                           AllowedOrigins = new string[] { "*" }
                       }
                   }));
            });

            return services;
        }
    }
}
