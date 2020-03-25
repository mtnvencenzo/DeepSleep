namespace DeepSleep.NetCore
{
    using DeepSleep.Formatting;
    using DeepSleep.Auth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using DeepSleep.Validation;
    using System;
    using DeepSleep.Pipeline;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Auth.Providers;
    using DeepSleep.NetCore.Controllers;
    using DeepSleep.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public static class ApiCoreHttpExtensionMethods
    {
        #region Helper Methods

        /// <summary>Gets the default validation provider.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiValidationProvider GetDefaultValidationProvider(IServiceProvider serviceProvider)
        {
            return new DefaultApiValidationProvider(serviceProvider)
                .RegisterInvoker<TypeBasedValidationInvoker>();
        }

        /// <summary>Gets the default response message processor provider.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiResponseMessageProcessorProvider GetDefaultResponseMessageProcessorProvider(IServiceProvider serviceProvider)
        {
            var provider = new DefaultApiResponseMessageProcessorProvider(serviceProvider)
                .RegisterProcessor<ApiResultResponseMessageProcessor>();

            return provider;
        }

        /// <summary>Gets the default formatter factory.</summary>
        /// <returns></returns>
        private static IFormatStreamReaderWriterFactory GetDefaultFormatterFactory()
        {
            return new HttpMediaTypeStreamWriterFactory()
                .Add(new JsonHttpFormatter(), new string[] { "application/json", "text/json", "application/json-patch+json" }, new string[] { "utf-32, utf-16, utf-8" })
                .Add(new XmlHttpFormatter(), new string[] { "text/xml", "application/xml" }, new string[] { "utf-32, utf-16, utf-8" });
        }

        /// <summary>Gets the default request pipeline.</summary>
        /// <returns></returns>
        private static IApiRequestPipeline GetDefaultRequestPipeline()
        {
            return new ApiRequestPipeline()
                .UseApiResponseUnhandledExceptionHandler()
                .UseApiRequestCanceled()
                .UseApiHttpComformance()
                .UseApiResponseBodyWriter()
                .UseApiResponseCookies()
                .UseApiResponseMessages()
                .UseApiResponseHttpCaching()
                .UseApiRequestUriValidation()
                .UseApiRequestHeaderValidation()
                .UseApiResponseCorrelation()
                .UseApiResponseDeprecated()
                .UseApiRequestRouting()
                .UseApiRequestLocalization()
                .UseApiRequestNotFound()
                .UseApiResponseCors()
                .UseApiRequestCorsPreflight()
                .UseApiRequestMethod()
                .UseApiRequestAccept()
                .UseApiRequestAuthentication()
                .UseApiRequestAuthorization()
                .UseApiRequestInvocationInitializer()
                .UseApiRequestUriBinding()
                .UseApiRequestBodyBinding()
                .UseApiRequestEndpointValidation()
                .UseApiRequestEndpointInvocation();
        }

        /// <summary>Gets the default API response message converter.</summary>
        /// <returns></returns>
        private static IApiResponseMessageConverter GetDefaultApiResponseMessageConverter()
        {
            return new DefaultApiResponseMessageConverter();
        }

        /// <summary>Gets the default routing table.</summary>
        /// <returns></returns>
        private static IApiRoutingTable GetDefaultRoutingTable()
        {
            return new DefaultApiRoutingTable();
        }

        /// <summary>Gets the default request contriguration.</summary>
        /// <returns></returns>
        private static IApiRequestConfiguration GetDefaultRequestContriguration()
        {
            return new DefaultApiRequestConfiguration
            {
                AllowAnonymous = false,
                CacheDirective = new HttpCacheDirective
                {
                    Cacheability = HttpCacheType.NoCache,
                    CacheLocation = HttpCacheLocation.Private,
                    ExpirationSeconds = -1
                },
                CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = false,
                    AllowedOrigins = new string[] { "*" },
                    ExposeHeaders = new string[] {      
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
                FallBackLanguage = null,
                HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = int.MaxValue
                },
                HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = false,
                    SupportedVersions = new string[] {       
                        "http/1.1",
                        "http/1.2",
                        "http/2",
                        "http/2.0",
                        "http/2.1"
                    }
                },
                MaxRequestLength = int.MaxValue,
                MaxRequestUriLength = int.MaxValue,
                MinRequestLength = 0,
                ResourceId = Guid.Empty.ToString(),
                SupportedLanguages = new string[] { }
            };
        }

        /// <summary>Adds the ping endpoint.</summary>
        /// <param name="table">The table.</param>
        /// <param name="config">The configuration.</param>
        private static void AddPingEndpoint(IApiRoutingTable table, IApiServiceConfiguration config)
        {
            table.AddRoute(
               template: $"ping",
               httpMethod: "GET",
               name: $"GET_ping",
               controller: typeof(PingController),
               endpoint: nameof(PingController.Ping),
               config: new DefaultApiRequestConfiguration
               {
                   AllowAnonymous = true,
                   CacheDirective = new HttpCacheDirective
                   {
                       Cacheability = HttpCacheType.NoCache,
                       CacheLocation = HttpCacheLocation.Private,
                       ExpirationSeconds = -5
                   },
                   CrossOriginConfig = new CrossOriginConfiguration
                   {
                       AllowCredentials = false,
                       AllowedOrigins = new string[] { "*" }
                   },
                   Deprecated = false,
                   ResourceId = $"{Guid.Empty}_Ping"
               });
        }

        /// <summary>Adds the ping endpoint.</summary>
        /// <param name="table">The table.</param>
        /// <param name="config">The configuration.</param>
        private static void AddEnvironmentEndpoint(IApiRoutingTable table, IApiServiceConfiguration config)
        {
            table.AddRoute(
               template: $"env",
               httpMethod: "GET",
               name: $"GET_env",
               controller: typeof(EnnvironmentController),
               endpoint: nameof(EnnvironmentController.Env),
               config: new DefaultApiRequestConfiguration
               {
                   AllowAnonymous = true,
                   CacheDirective = new HttpCacheDirective
                   {
                       Cacheability = HttpCacheType.NoCache,
                       CacheLocation = HttpCacheLocation.Private,
                       ExpirationSeconds = -5
                   },
                   CrossOriginConfig = new CrossOriginConfiguration
                   {
                       AllowCredentials = false,
                       AllowedOrigins = new string[] { "*" }
                   },
                   Deprecated = false,
                   ResourceId = $"{Guid.Empty}_Environment"
               });
        }

        #endregion

        /// <summary>Uses the API request context.</summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiCoreHttp(this IApplicationBuilder builder)
        {
            return builder
                .UseApiRequestContext();
        }

        /// <summary>Uses the API core services.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection UseApiCoreServices(this IServiceCollection services, IApiServiceConfiguration config)
        {
            services
                .AddScoped<IApiRequestContextResolver, DefaultApiRequestContextResolver>()
                .AddScoped<IApiRequestContextResolver, DefaultApiRequestContextResolver>()
                .AddScoped<IApiRequestContextResolver, DefaultApiRequestContextResolver>()
                .AddScoped<IUriRouteResolver, DefaultRouteResolver>()
                .AddScoped<IApiValidationProvider, IApiValidationProvider>((p) => config.ApiValidationProvider ?? GetDefaultValidationProvider(p))
                .AddScoped<IApiResponseMessageConverter, IApiResponseMessageConverter>((p) => config.ApiResponseMessageConverter ?? GetDefaultApiResponseMessageConverter())
                .AddScoped<IFormatStreamReaderWriterFactory, IFormatStreamReaderWriterFactory>((p) => config.FormatterFactory ?? GetDefaultFormatterFactory())
                .AddSingleton<IApiRequestPipeline, IApiRequestPipeline>((p) => config.ApiRequestPipeline ?? GetDefaultRequestPipeline())
                .AddScoped<IApiResponseMessageProcessorProvider, IApiResponseMessageProcessorProvider>((p) => config.ApiResponseMessageProcessorProvider ?? GetDefaultResponseMessageProcessorProvider(p))
                .AddSingleton<IApiRequestConfiguration, IApiRequestConfiguration>((p) => config.DefaultRequestConfiguration ?? GetDefaultRequestContriguration())
                .AddSingleton<IApiServiceConfiguration, IApiServiceConfiguration>((p) => config);

            services.AddSingleton<IApiRoutingTable, IApiRoutingTable>((p) =>
            {
                var routingTable = config.RoutingTable ?? GetDefaultRoutingTable();

                if (config.UsePingEndpoint)
                {
                    AddPingEndpoint(routingTable, config);
                }

                if (config.UseEnvironmentEndpoint)
                {
                    AddEnvironmentEndpoint(routingTable, config);
                }

                return routingTable;
            });


            return services;
        }
    }
}
