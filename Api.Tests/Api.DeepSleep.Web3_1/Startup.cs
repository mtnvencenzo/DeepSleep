namespace Api.DeepSleep.Web3_1
{
    using Api.DeepSleep.Controllers;
    using global::DeepSleep.Validation;
    using global::DeepSleep.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
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
            ServiceStartup.RegisterServices(services);

            services
                .AddLogging()
                .UseOpenApiServices(
                    info: null,
                    v2RouteTemplate: "openapi/v2/doc",
                    v3RouteTemplate: "openapi/v3/doc",
                    prefixNamesWithNamespace: false,
                    includeHeadOperationsForGets: true,
                    xmlDocumentationFileNames: new List<string>
                    {
                        "Api.DeepSleep.Models.xml",
                        "Api.DeepSleep.Controllers.xml",
                        "deepsleep.xml",
                        "deepsleep.web.xml"
                    })
                .UseDataAnnotationValidations(continuation: ValidationContinuation.OnlyIfValid, validateAllProperties: true)
                .UseApiCoreServices(new DefaultApiServiceConfiguration
                {
                    DiscoveryStrategies = ServiceStartup.DiscoveryStrategies(),
                    DefaultRequestConfiguration = ServiceStartup.DefaultRequestConfiguration(),
                    PingEndpoint = new EndpointUsage
                    {
                        Enabled = true,
                        RelativePath = "ping"
                    },
                    WriteConsoleHeader = false,
                    OnException = (ctx, ex) =>
                    {
                        var context = ctx.GetContext();
                        if (context.Items.ContainsKey("exceptionHandlerCount"))
                        {
                            var count = (int)context.Items["exceptionHandlerCount"];
                            context.Items["exceptionHandlerCount"] = count++;
                        }
                        else
                        {
                            context.Items["exceptionHandlerCount"] = 1;
                        }

                        return Task.CompletedTask;
                    },
                    OnRequestProcessed = (ctx) =>
                    {
                        var context = ctx.GetContext();
                        if (context.Items.ContainsKey("requestHandlerCount"))
                        {
                            var count = (int)context.Items["requestHandlerCount"];
                            context.Items["requestHandlerCount"] = count++;
                        }
                        else
                        {
                            context.Items["requestHandlerCount"] = 1;
                        }

                        return Task.CompletedTask;
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
    }
}
