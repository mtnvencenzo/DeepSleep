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
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;

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

            config.RoutingTable ??= GetDefaultRoutingTable();

            if (config.UsePingEndpoint)
            {
                AddPingEndpoint(config.RoutingTable);
            }

            if (config.UseEnvironmentEndpoint)
            {
                AddEnvironmentEndpoint(config.RoutingTable);
            }

            services.AddSingleton<IApiRoutingTable, IApiRoutingTable>((p) => config.RoutingTable);


            try
            {
                WriteDeepsleepToConsole(config);
            }
            catch { }

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
        private static void AddPingEndpoint(IApiRoutingTable table)
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

        /// <summary>Adds the environment endpoint.</summary>
        /// <param name="table">The table.</param>
        private static void AddEnvironmentEndpoint(IApiRoutingTable table)
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

        /// <summary>Writes the deepsleep to console.</summary>
        /// <param name="config">The configuration.</param>
        private static void WriteDeepsleepToConsole(IApiServiceConfiguration config)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var existingColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($@"");
            Console.WriteLine($@" ____                  _             ");
            Console.WriteLine($@"|    \ ___ ___ ___ ___| |___ ___ ___ ");
            Console.WriteLine($@"|  |  | -_| -_| . |_ -| | -_| -_| . |");
            Console.WriteLine($@"|____/|___|___|  _|___|_|___|___|  _|");
            Console.Write($@"              |_|               |_|  ");
            Console.ForegroundColor = existingColor;

            Console.WriteLine($"   v{version}");
            Console.WriteLine($"");
            Console.WriteLine($"------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Endpoints: ");
            Console.ForegroundColor = existingColor;
            Console.WriteLine($"{config?.RoutingTable?.GetRoutes()?.Count ?? 0}");

            Console.WriteLine($"------------------------------------------------");
            Console.WriteLine($"");

            var routes = (config?.RoutingTable?.GetRoutes() ?? new List<ApiRoutingItem>())
                .OrderBy(r => r.Template)
                .ToList();

            routes.ForEach(r =>
            {
                existingColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  {r.HttpMethod.ToUpper().PadRight(9, ' ')}");
                Console.ForegroundColor = existingColor;
                Console.WriteLine($"{r.Template}");
            });

            Console.WriteLine("");

            MayTheFourth();

            Console.WriteLine();
        }

        /// <summary>Mays the fourth.</summary>
        private static void MayTheFourth()
        {
            var now = DateTime.Now;

            if (now.Month == 5 && now.Day == 4)
            {
                var existingColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine(@"                       _.-'~~~~~~`-._");
                Console.WriteLine(@"                      /      ||      \");
                Console.WriteLine(@"                     /       ||       \");
                Console.WriteLine(@"                    |        ||        |");
                Console.WriteLine(@"                    | _______||_______ |");
                Console.WriteLine(@"                    |/ ----- \/ ----- \|");
                Console.WriteLine(@"                   /  (     )  (     )  \");
                Console.WriteLine(@"                  / \  ----- () -----  / \");
                Console.WriteLine(@"                 /   \      /||\      /   \");
                Console.WriteLine(@"                /     \    /||||\    /     \");
                Console.WriteLine(@"               /       \  /||||||\  /       \");
                Console.WriteLine(@"              /_        \o========o/        _\");
                Console.WriteLine(@"                `--...__|`-._  _.-'|__...--'");
                Console.WriteLine(@"                        |    `'    |");

                
                Console.WriteLine("");
                Console.WriteLine(@"                  May the 4th be with you!");
                Console.WriteLine("");
                Console.ForegroundColor = existingColor;
            }
        }
    }
}
