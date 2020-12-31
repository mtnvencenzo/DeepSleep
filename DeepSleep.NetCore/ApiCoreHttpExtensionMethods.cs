namespace DeepSleep.NetCore
{
    using DeepSleep.Configuration;
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.NetCore.Controllers;
    using DeepSleep.Validation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Threading.Tasks;

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
            var ret = builder
                .UseApiRequestContext();

            var config = builder.ApplicationServices.GetService<IApiServiceConfiguration>();
            var routingTable = builder.ApplicationServices.GetService<IApiRoutingTable>();

            if (config.DiscoveryStrategies != null)
            {
                foreach (var strategy in config.DiscoveryStrategies)
                {
                    if (strategy == null)
                    {
                        continue;
                    }

                    var task = Task.Run(async () =>
                    {
                        return await strategy.DiscoverRoutes(builder.ApplicationServices).ConfigureAwait(false);
                    });

                    var registrations = task.Result;

                    foreach (var registration in registrations)
                    {
                        if (registration == null)
                        {
                            continue;
                        }

                        routingTable.AddRoute(
                            template: registration.Template,
                            httpMethod: registration.HttpMethod,
                            controller: registration.Location.Controller,
                            endpoint: registration.Location.Endpoint,
                            config: registration.Configuration);
                    }
                }
            }


            if (config as DefaultApiServiceConfiguration != null)
            {
                if (((DefaultApiServiceConfiguration)config).UsePingEndpoint)
                {
                    AddPingEndpoint(routingTable);
                }
            }

            try
            {
                WriteDeepsleepToConsole(routingTable);
            }
            catch { }

            return ret;
        }

        /// <summary>Uses the API core services.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection UseApiCoreServices(this IServiceCollection services, IApiServiceConfiguration config)
        {
            var routingTable = new DefaultApiRoutingTable();

            services
                .AddScoped<IApiRequestContextResolver, DefaultApiRequestContextResolver>()
                .AddScoped<IFormUrlEncodedObjectSerializer, FormUrlEncodedObjectSerializer>()
                .AddScoped<IUriRouteResolver, DefaultRouteResolver>()
                .AddScoped<IApiValidationProvider, IApiValidationProvider>((p) => config.ApiValidationProvider ?? GetDefaultValidationProvider(p))
                .AddScoped<IJsonFormattingConfiguration, IJsonFormattingConfiguration>((p) => config.JsonConfiguration ?? GetDefaultJsonFormattingConfiguration())
                .AddScoped<IFormatStreamReaderWriter, JsonHttpFormatter>()
                .AddScoped<IFormatStreamReaderWriter, XmlHttpFormatter>()
                .AddScoped<IFormatStreamReaderWriter, FormUrlEncodedFormatter>()
                .AddScoped<IFormatStreamReaderWriter, MultipartFormDataFormatter>()
                .AddScoped<IMultipartStreamReader, MultipartStreamReader>()
                .AddScoped<IFormatStreamReaderWriterFactory, HttpMediaTypeStreamReaderWriterFactory>()
                .AddSingleton<IApiRequestPipeline, IApiRequestPipeline>((p) => DefaultApiServiceConfiguration.GetDefaultRequestPipeline())
                .AddSingleton<IApiRequestConfiguration, IApiRequestConfiguration>((p) => config.DefaultRequestConfiguration ?? ApiRequestContext.GetDefaultRequestConfiguration())
                .AddSingleton<IApiServiceConfiguration, IApiServiceConfiguration>((p) => config)
                .AddSingleton<IApiRoutingTable, IApiRoutingTable>((p) => routingTable);

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

        /// <summary>Adds the ping endpoint.</summary>
        /// <param name="table">The table.</param>
        private static void AddPingEndpoint(IApiRoutingTable table)
        {
            table.AddRoute(
               template: $"ping",
               httpMethod: "GET",
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
                   CrossOriginConfig = new ApiCrossOriginConfiguration
                   {
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
        /// <param name="routingTable">The routing table.</param>
        private static void WriteDeepsleepToConsole(IApiRoutingTable routingTable)
        {
            var deepSleepNetCoreAssembly = Assembly.GetExecutingAssembly();

            var deepSleepNetCoreTargetFrameworkAttribute = deepSleepNetCoreAssembly
                .GetCustomAttributes(true)
                .OfType<TargetFrameworkAttribute>()
                .FirstOrDefault();

            var deepSleepNetCoreTargetDisplay = !string.IsNullOrWhiteSpace(deepSleepNetCoreTargetFrameworkAttribute?.FrameworkName)
                ? deepSleepNetCoreTargetFrameworkAttribute.FrameworkName
                : deepSleepNetCoreTargetFrameworkAttribute?.FrameworkDisplayName;

            var deepSleepNetCoreAssemblyName = deepSleepNetCoreAssembly.GetName();
            var deepSleepNetCoreVersion = deepSleepNetCoreAssemblyName.Version;


            var deepSleepAssembly = Assembly.GetAssembly(typeof(ApiRequestContext));

            var deepSleepTargetFrameworkAttribute = deepSleepAssembly
                .GetCustomAttributes(true)
                .OfType<TargetFrameworkAttribute>()
                .FirstOrDefault();

            var deepSleepTargetDisplay = !string.IsNullOrWhiteSpace(deepSleepTargetFrameworkAttribute?.FrameworkName)
                ? deepSleepTargetFrameworkAttribute.FrameworkName
                : deepSleepTargetFrameworkAttribute?.FrameworkDisplayName;

            var deepSleepAssemblyName = deepSleepAssembly.GetName();
            var deepSleepVersion = deepSleepAssemblyName.Version;

            var existingColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($@"");
            Console.WriteLine($@" ____                  _             ");
            Console.WriteLine($@"|    \ ___ ___ ___ ___| |___ ___ ___ ");
            Console.WriteLine($@"|  |  | -_| -_| . |_ -| | -_| -_| . |");
            Console.WriteLine($@"|____/|___|___|  _|___|_|___|___|  _|");
            Console.Write($@"              |_|               |_|  ");
            Console.ForegroundColor = existingColor;

            Console.WriteLine($"   v{deepSleepVersion}");
            Console.WriteLine($"");

            if (!string.IsNullOrWhiteSpace(deepSleepNetCoreTargetDisplay))
            {
                Console.WriteLine($"");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Target(s): ");
                Console.ForegroundColor = existingColor;
                Console.WriteLine($"------------------------------------------------");
                Console.ForegroundColor = existingColor;
                Console.WriteLine($"           {deepSleepAssemblyName.Name}, {deepSleepAssemblyName.Version}, {deepSleepTargetDisplay}");
                Console.WriteLine($"           {deepSleepNetCoreAssemblyName.Name}, {deepSleepNetCoreAssemblyName.Version}, {deepSleepNetCoreTargetDisplay}");
                Console.WriteLine($"");
            }


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Endpoints: ");
            Console.ForegroundColor = existingColor;
            Console.WriteLine($"{routingTable?.GetRoutes()?.Count ?? 0}");
            Console.WriteLine($"------------------------------------------------");
            Console.WriteLine($"");

            var routes = (routingTable?.GetRoutes() ?? new List<ApiRoutingItem>())
                .OrderBy(r => r.Template)
                .ToList();

            routes.ForEach(r =>
            {
                existingColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  {r.HttpMethod.ToUpper().PadRight(9, ' ')}");
                Console.ForegroundColor = existingColor;

                WriteEndpointTemplate(r.Template);
            });

            Console.WriteLine("");

            MayTheFourth();

            Console.WriteLine();
        }

        /// <summary>Writes the endpoint template.</summary>
        /// <param name="template">The template.</param>
        private static void WriteEndpointTemplate(string template)
        {
            var existingColor = Console.ForegroundColor;

            foreach (var c in template.ToCharArray())
            {
                if (c == '{')
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                Console.Write(c);

                if (c == '}')
                {
                    Console.ForegroundColor = existingColor;
                }
            }

            Console.WriteLine("");
            Console.ForegroundColor = existingColor;
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
