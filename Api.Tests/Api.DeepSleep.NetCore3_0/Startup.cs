namespace Api.DeepSleep.NetCore3_0
{
    using Api.DeepSleep.Controllers;
    using global::DeepSleep.NetCore;
    using global::DeepSleep.OpenApi.NetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public class Startup
    {
        private IServiceProvider serviceProvider;

        /// <summary>Initializes a new instance of the <see cref="Startup" /> class.</summary>
        /// <param name="env">The env.</param>
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName?.ToLower()}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        public IServiceProvider ServiceProvider => this.serviceProvider!;

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>Configures the services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureDependentServices(services);
        }

        /// <summary>Configures the dependent services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureDependentServices(IServiceCollection services)
        {
            ServiceStartup.RegisterServices(services);

            services
                .AddLogging()
                .UseOpenApiServices()
                .UseApiCoreServices(new DefaultApiServiceConfiguration
                {
                    DiscoveryStrategies = ServiceStartup.DiscoverRoutes(),
                    ApiValidationProvider = ServiceStartup.DefaultValidationProvider(this.serviceProvider),
                    DefaultRequestConfiguration = ServiceStartup.DefaultRequestConfiguration(),
                    UsePingEndpoint = true,
                    WriteConsoleHeader = false
                });


            if (ServicePreprocessor != null)
            {
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
                this.serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

                ServicePreprocessor(services);
            }

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            this.serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        public void Configure(IApplicationBuilder app)
        {
            app
                .UseOpenApiEndpoint(
                    routeTemplate: "openapi/v3/doc",
                    prefixNamesWithNamespace: false,
                    includeHeadOperationsForGets: true)
                .UseApiCoreHttp()
                .UseForwardedHeaders();
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<IServiceCollection> ServicePreprocessor { get; set; }
    }
}
