namespace DeepSleep.Web
{
    using DeepSleep.Health;
    using DeepSleep.Web.Controllers;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    public static class PingEndpointHttpExtensionMethods
    {
        /// <summary>Uses the ping endpoint.</summary>
        /// <param name="services">The services.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepPingEndpoint(
            this IServiceCollection services, 
            string routeTemplate)
        {
            services.AddSingleton<IPingEndpointConfigurationProvider>(new PingEndpointConfigurationProvider
            {
                Template = routeTemplate
            });

            return services.AddTransient<PingController>();
        }
    }
}
