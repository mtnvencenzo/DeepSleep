namespace DeepSleep.Api.Harness
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using DeepSleep;
    using DeepSleep.NetCore;
    using DeepSleep.Validation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using DeepSleep.OpenApi.NetCore;
    using DeepSleep.Configuration;
    using System.Collections.Generic;
    using DeepSleep.Auth;
    using DeepSleep.Api.Harness.Controllers;

    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        private IServiceProvider serviceProvider;
        private ILogger logger = null;

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
            services.AddTransient<EnnvironmentController>();

  
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
            this.serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'

            this.logger = this.serviceProvider.GetService<ILogger<Startup>>();

            services
                .AddLogging()
                .UseOpenApiServices()
                .UseApiCoreServices(new DefaultApiServiceConfiguration
                {
                    RoutingTable = GetRoutes(this.serviceProvider),
                    ExceptionHandler = ExceptionHandler,
                    ApiValidationProvider = new DefaultApiValidationProvider(serviceProvider).RegisterInvoker<TypeBasedValidationInvoker>(),
                    DefaultRequestConfiguration = new DefaultApiRequestConfiguration
                    {
                        AllowAnonymous = false,
                        CacheDirective = new HttpCacheDirective
                        {
                            Cacheability = HttpCacheType.NoCache,
                            CacheLocation = HttpCacheLocation.Private,
                            ExpirationSeconds = -5
                        },
                        CrossOriginConfig = new CrossOriginConfiguration
                        {
                            AllowCredentials = true,
                            ExposeHeaders = new string[]
                            {
                                "X-CorrelationId",
                                "X-Deprecated",
                                "X-Api-Message",
                                "X-Api-Version",
                                "X-Api-RequestId",
                                "X-Allow-Accept",
                                "X-Allow-Accept-Charset",
                                "X-PrettyPrint",
                                "Location"
                            }
                        },
                        Deprecated = false,
                        FallBackLanguage = "en-US",
                        HeaderValidationConfig = new ApiHeaderValidationConfiguration
                        {
                            MaxHeaderLength = int.MaxValue
                        },
                        SupportedLanguages = new string[] { "en-US", "de-DE" },
                    }
                });
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        /// <param name="loggerFactory">The logger factory</param>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app
                .UseApiCoreHttp()
                .UseForwardedHeaders();
        }

        /// <summary>Gets the exception handler.</summary>
        /// <value>The exception handler.</value>
        public Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler
        {
            get
            {
                return new Func<ApiRequestContext, Exception, Task<long>>((c, e) =>
                {
                    this.logger?.LogError(e, "Unandled exception retreived");
                    return Task.FromResult((long)0);
                });
            }
        }

        /// <summary>Gets the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private IApiRoutingTable GetRoutes(IServiceProvider serviceProvider)
        {
            var table = new DefaultApiRoutingTable();

            table.AddRoute(
                template: $"environment/server",
                httpMethod: "GET",
                name: $"GET_environment/server",
                controller: typeof(EnnvironmentController),
                endpoint: nameof(EnnvironmentController.Env),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    ResourceId = "464AA6A7-6F6A-4BB9-A941-07BB7748EB48",
                    ResourceAuthorizationConfig = new ResourceAuthorizationConfiguration()
                });

            table.AddRoute(
                template: $"test",
                httpMethod: "POST",
                name: $"POST_test",
                controller: typeof(TestController),
                endpoint: nameof(TestController.Post),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    ResourceId = "134C800C-71D8-4203-AAFF-B8E9E462663E",
                    ResourceAuthorizationConfig = new ResourceAuthorizationConfiguration()
                });

            return table;
        }
    }
}
