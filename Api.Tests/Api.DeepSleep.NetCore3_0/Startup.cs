namespace Api.DeepSleep.NetCore3_0
{
    using Api.DeepSleep.Controllers.Binding;
    using global::DeepSleep;
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Formatting;
    using global::DeepSleep.NetCore;
    using global::DeepSleep.Validation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
            // Controlllers
            services.AddTransient<SimpleUrlBindingController>();

            services
                .AddLogging()
                .UseApiCoreServices(new DefaultApiServiceConfiguration
                {
                    RoutingTable = GetRoutes(this.serviceProvider),
                    ApiValidationProvider = new DefaultApiValidationProvider(serviceProvider).RegisterInvoker<TypeBasedValidationInvoker>(),
                    DefaultRequestConfiguration = new DefaultApiRequestConfiguration
                    {
                        AllowAnonymous = true
                    }
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
                .UseApiCoreHttp()
                .UseForwardedHeaders();
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<IServiceCollection> ServicePreprocessor { get; set; }

        /// <summary>Gets the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiRoutingTable GetRoutes(IServiceProvider serviceProvider)
        {
            var table = new DefaultApiRoutingTable();

            table.AddRoute(
                template: $"binding/simple/url",
                httpMethod: "GET",
                name: $"GET_binding/simple/url",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithQuery),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "binding/simple/url/{stringVar}/resource",
                httpMethod: "GET",
                name: "GET_binding/simple/url/{stringVar}/resource",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithRoute),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "binding/simple/url/{stringVar}/mixed",
                httpMethod: "GET",
                name: "GET_binding/simple/url/{stringVar}/mixed",
                controller: typeof(SimpleUrlBindingController),
                endpoint: nameof(SimpleUrlBindingController.GetWithMixed),
                config: new DefaultApiRequestConfiguration());

            return table;
        }
    }
}
