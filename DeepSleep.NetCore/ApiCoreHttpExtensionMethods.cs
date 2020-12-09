namespace DeepSleep.NetCore
{
    using DeepSleep.Formatting;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using DeepSleep.Validation;
    using System;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.NetCore.Controllers;
    using DeepSleep.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public static class ApiCoreHttpExtensionMethods
    {
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
                .AddTransient<IFormUrlEncodedObjectSerializer, FormUrlEncodedObjectSerializer>()
                .AddScoped<IUriRouteResolver, DefaultRouteResolver>()
                .AddScoped<IApiValidationProvider, IApiValidationProvider>((p) => config.ApiValidationProvider ?? GetDefaultValidationProvider(p))
                .AddScoped<IJsonFormattingConfiguration, IJsonFormattingConfiguration>((p) => config.JsonConfiguration ?? GetDefaultJsonFormattingConfiguration())
                .AddScoped<IFormatStreamReaderWriter, JsonHttpFormatter>()
                .AddScoped<IFormatStreamReaderWriter, XmlHttpFormatter>()
                .AddScoped<IFormatStreamReaderWriter, FormUrlEncodedFormatter>()
                .AddScoped<IFormatStreamReaderWriter, MultipartFormDataFormatter>()
                .AddSingleton<IApiRequestPipeline, IApiRequestPipeline>((p) => config.ApiRequestPipeline ?? DefaultApiServiceConfiguration.GetDefaultRequestPipeline())
                .AddSingleton<IApiRequestConfiguration, IApiRequestConfiguration>((p) => config.DefaultRequestConfiguration ?? GetDefaultRequestConfiguration())
                .AddSingleton<IApiServiceConfiguration, IApiServiceConfiguration>((p) => config)
                .AddScoped<IMultipartStreamReader, MultipartStreamReader>();

            if (config.FormatterFactory != null)
            {
                services.AddScoped<IFormatStreamReaderWriterFactory, IFormatStreamReaderWriterFactory>((p) => config.FormatterFactory);
            }
            else
            {
                services.AddScoped<IFormatStreamReaderWriterFactory, HttpMediaTypeStreamWriterFactory>();
            }

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

        /// <summary>Gets the default validation provider.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiValidationProvider GetDefaultValidationProvider(IServiceProvider serviceProvider)
        {
            return new DefaultApiValidationProvider(serviceProvider)
                .RegisterInvoker<TypeBasedValidationInvoker>();
        }

        /// <summary>Gets the default routing table.</summary>
        /// <returns></returns>
        private static IApiRoutingTable GetDefaultRoutingTable()
        {
            return new DefaultApiRoutingTable();
        }

        /// <summary>Gets the default request contriguration.</summary>
        /// <returns></returns>
        private static IApiRequestConfiguration GetDefaultRequestConfiguration()
        {
            return new DefaultApiRequestConfiguration
            {
                AllowAnonymous = false,
                ApiErrorResponseProvider = (p) => new ApiResultErrorResponseProvider
                {
                    WriteToBody = true,
                    WriteToHeaders = false
                },
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
                   Deprecated = false
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
                   Deprecated = false
               });
        }

        /// <summary>Gets the default json formatting configuration
        /// </summary>
        /// <returns></returns>
        private static IJsonFormattingConfiguration GetDefaultJsonFormattingConfiguration()
        {
            return new JsonFormattingConfiguration
            {
                CasingStyle = FormatCasingStyle.CamelCase,
                NullValuesExcluded = true
            };
        }
    }
}
