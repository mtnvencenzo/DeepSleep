namespace DeepSleep.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Media;
    using DeepSleep.Media.Converters;
    using DeepSleep.Media.Serializers;
    using DeepSleep.Health;
    using DeepSleep.OpenApi;
    using DeepSleep.Pipeline;
    using DeepSleep.Validation;
    using DeepSleep.Web.Controllers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// 
    /// </summary>
    public static class ApiCoreHttpExtensionMethods
    {
        /// <summary>Uses the API request context.</summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseDeepSleep(this IApplicationBuilder builder)
        {
            builder.UseApiRequestContext();

            var config = builder.ApplicationServices.GetService<IDeepSleepServiceConfiguration>();
            var routingTable = builder.ApplicationServices.GetService<IApiRoutingTable>();

            DiscoverRoutes(builder, routingTable, config);


            // ---------------------------------
            // Setup Ping Endpoint if being used
            // ---------------------------------
            IPingEndpointConfigurationProvider pingConfiguration = null;

            try
            {
                pingConfiguration = builder.ApplicationServices.GetService<IPingEndpointConfigurationProvider>();
            }
            catch { }

            if (pingConfiguration != null)
            {
                AddPingEndpoint(routingTable, pingConfiguration);
            }


            // ---------------------------------
            // Setup OpenApi Endpoint(s) if being used
            // ---------------------------------
            IDeepSleepOasConfigurationProvider openApiConfiguration = null;

            try
            {
                openApiConfiguration = builder.ApplicationServices.GetService<IDeepSleepOasConfigurationProvider>();
            }
            catch { }

            if (openApiConfiguration != null)
            {
                AddOpenApiEndpoints(routingTable, openApiConfiguration);
            }

            // ---------------------------------
            // Setup Console header if being used
            // ---------------------------------
            if (config?.WriteConsoleHeader ?? true)
            {
                try
                {
                    WriteDeepsleepToConsole(routingTable);
                }
                catch { }
            }

            return builder;
        }

        /// <summary>Uses the deep sleep services.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepServices(this IServiceCollection services, Action<IDeepSleepServiceConfiguration> configure = null)
        {
            var routingTable = new ApiRoutingTable();
            var configuration = new DeepSleepServiceConfiguration
            {
                DefaultRequestConfiguration = ApiRequestContext.GetDefaultRequestConfiguration(),
                ExcludePaths = new List<string>(),
                WriteConsoleHeader = true,
                DiscoveryStrategies = new List<IDeepSleepDiscoveryStrategy>()
            };

            if (configure != null)
            {
                configure(configuration);
            }

            configuration.DefaultRequestConfiguration = configuration.DefaultRequestConfiguration ?? ApiRequestContext.GetDefaultRequestConfiguration();
            configuration.ExcludePaths = configuration.ExcludePaths ?? new List<string>();

            services
                .AddScoped<IApiRequestContextResolver, ApiRequestContextResolver>()
                .AddScoped<IFormUrlEncodedObjectSerializer, FormUrlEncodedObjectSerializer>()
                .AddScoped<IUriRouteResolver, ApiRouteResolver>()
                .AddScoped<IMultipartStreamReader, MultipartStreamReader>()
                .AddScoped<IDeepSleepMediaSerializerFactory, DeepSleepMediaSerializerWriterFactory>()
                .AddScoped<IApiValidationProvider, ApiEndpointValidationProvider>()
                .AddSingleton<IApiRequestPipeline, IApiRequestPipeline>((p) => ApiRequestPipeline.GetDefaultRequestPipeline())
                .AddSingleton<IDeepSleepRequestConfiguration, IDeepSleepRequestConfiguration>((p) => configuration.DefaultRequestConfiguration)
                .AddSingleton<IDeepSleepServiceConfiguration, IDeepSleepServiceConfiguration>((p) => configuration)
                .AddSingleton<IApiRoutingTable, IApiRoutingTable>((p) => routingTable);

            return services;
        }

        /// <summary>Uses the deep sleep json negotiation.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepJsonNegotiation(this IServiceCollection services, Action<JsonMediaSerializerConfiguration> configure = null)
        {
            var configuration = new JsonMediaSerializerConfiguration
            {
                SerializerOptions = GetJsonSerializerSettings()
            };

            if (configure != null)
            {
                configure(configuration);
            }

            services.AddScoped<IDeepSleepMediaSerializer, DeepSleepJsonMediaSerializer>();
            services.AddSingleton<JsonMediaSerializerConfiguration>((p) => configuration);

            return services;
        }

        /// <summary>Uses the deep sleep multipart form data.</summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepMultipartFormDataNegotiation(this IServiceCollection services)
        {
            services.AddScoped<IDeepSleepMediaSerializer, DeepSleepMultipartFormDataMediaSerializer>();

            return services;
        }

        /// <summary>Uses the deep sleep XML negotiation.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepXmlNegotiation(this IServiceCollection services, Action<XmlMediaSerializerConfiguration> configure = null)
        {
            var configuration = new XmlMediaSerializerConfiguration
            {
                ReaderSerializerSettings = new XmlReaderSettings
                {
                    CloseInput = false,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    IgnoreComments = true,
                    ValidationType = ValidationType.None
                },
                WriterSerializerSettings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    CloseOutput = false,
                    Encoding = Encoding.UTF8,
                    Indent = false,
                    NamespaceHandling = NamespaceHandling.Default,
                    OmitXmlDeclaration = true,
                    WriteEndDocumentOnClose = false,
                    Async = true,
                }
            };

            if (configure != null)
            {
                configure(configuration);
            }

            services.AddScoped<IDeepSleepMediaSerializer, DeepSleepXmlMediaSerializer>();
            services.AddSingleton<XmlMediaSerializerConfiguration>((p) => configuration);

            return services;
        }

        /// <summary>Uses the deep sleep form URL encoded negotiation.</summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepFormUrlEncodedNegotiation(this IServiceCollection services)
        {
            services.AddScoped<IDeepSleepMediaSerializer, DeepSleepFormUrlEncodedMediaSerializer>();

            return services;
        }


        /// <summary>Gets the default routing table.</summary>
        /// <returns></returns>
        private static IApiRoutingTable GetDefaultRoutingTable()
        {
            return new ApiRoutingTable();
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="config">The configuration.</param>
        private static void DiscoverRoutes(IApplicationBuilder builder, IApiRoutingTable routingTable, IDeepSleepServiceConfiguration config)
        {
            var discoveryStrategies = config?.DiscoveryStrategies == null || config.DiscoveryStrategies.Count == 0
                ? DiscoveryStrategies.Default()
                : config.DiscoveryStrategies.Where(d => d != null).ToList();

            foreach (var strategy in discoveryStrategies)
            {
                if (strategy == null)
                {
                    continue;
                }

                var task = Task.Run(async () =>
                {
                    using (var scope = builder.ApplicationServices.CreateScope())
                    {
                        return await strategy.DiscoverRoutes(scope.ServiceProvider).ConfigureAwait(false);
                    }
                });

                var registrations = task.Result;

                foreach (var registration in registrations)
                {
                    if (registration == null)
                    {
                        continue;
                    }

                    routingTable.AddRoute(registration);
                }
            }
        }

        /// <summary>Adds the open API endpoints.</summary>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="openApiConfiguration">The open API configuration.</param>
        private static void AddOpenApiEndpoints(IApiRoutingTable routingTable, IDeepSleepOasConfigurationProvider openApiConfiguration)
        {
            Action<string, string> add = (string endpoint, string template) =>
            {
                routingTable.AddRoute(new DeepSleepRouteRegistration(
                    template: template,
                    httpMethods: new[] { "GET" },
                    controller: Type.GetType(typeof(OasController).AssemblyQualifiedName),
                    endpoint: endpoint,
                    config: new DeepSleepRequestConfiguration
                    {
                        AllowAnonymous = true,
                        ReadWriteConfiguration = new ApiMediaSerializerConfiguration
                        {
                            AcceptHeaderFallback = "application/json; q=1.0, application/yaml; q=0.9",
                            WriterResolver = (serviceProvider) =>
                            {
                                var jsonFormatter = serviceProvider.GetService<DeepSleepOasJsonFormatter>();
                                var yamlFormatter = serviceProvider.GetService<DeepSleepOasYamlFormatter>();

                                return Task.FromResult(new MediaSerializerWriteOverrides(new List<IDeepSleepMediaSerializer>
                                {
                                    jsonFormatter,
                                    yamlFormatter
                                }));
                            }
                        }
                    }));
            };

            if (!string.IsNullOrWhiteSpace(openApiConfiguration.V2RouteTemplate))
            {
                add(nameof(OasController.DocV2), openApiConfiguration.V2RouteTemplate);
            }

            if (!string.IsNullOrWhiteSpace(openApiConfiguration.V3RouteTemplate))
            {
                add(nameof(OasController.DocV3), openApiConfiguration.V3RouteTemplate);
            }
        }

        /// <summary>Adds the ping endpoint.</summary>
        /// <param name="table">The table.</param>
        /// <param name="pingConfiguration">The ping configuration.</param>
        private static void AddPingEndpoint(IApiRoutingTable table, IPingEndpointConfigurationProvider pingConfiguration)
        {
            string template = pingConfiguration?.Template ?? "ping";

            table.AddRoute(new DeepSleepRouteRegistration(
               template: template,
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
                   },
                   Deprecated = false
               }));
        }

        /// <summary>Writes the deepsleep to console.</summary>
        /// <param name="routingTable">The routing table.</param>
        internal static void WriteDeepsleepToConsole(IApiRoutingTable routingTable)
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
                Console.Write($"  {r.HttpMethod.ToUpper(),-9}");
                Console.ForegroundColor = existingColor;

                WriteEndpointTemplate(r.Template);
            });

            Console.WriteLine("");

            var now = DateTime.Now;
            if (now.Month == 5 && now.Day == 4)
            {
                MayTheFourth();
            }

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

        /// <summary>Gets the writer settings.</summary>
        /// <returns></returns>
        private static JsonSerializerOptions GetJsonSerializerSettings()
        {
            var settings = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                AllowTrailingCommas = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IgnoreReadOnlyProperties = false,
                IgnoreReadOnlyFields = true,
                IncludeFields = false,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = false
            };

            settings.Converters.Add(new NullableBooleanConverter());
            settings.Converters.Add(new BooleanConverter());
            settings.Converters.Add(new JsonStringEnumConverter(namingPolicy: new OasDefaultNamingPolicy(), allowIntegerValues: true));
            settings.Converters.Add(new NullableTimeSpanConverter());
            settings.Converters.Add(new TimeSpanConverter());
            settings.Converters.Add(new NullableDateTimeConverter());
            settings.Converters.Add(new DateTimeConverter());
            settings.Converters.Add(new NullableDateTimeOffsetConverter());
            settings.Converters.Add(new DateTimeOffsetConverter());
            settings.Converters.Add(new ContentDispositionConverter());
            settings.Converters.Add(new ContentTypeConverter());
            settings.Converters.Add(new ObjectConverter());

            return settings;
        }

        /// <summary>Mays the fourth.</summary>
        internal static void MayTheFourth()
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
            Console.WriteLine(@"                 May the fourth be with you!");
            Console.WriteLine("");
            Console.ForegroundColor = existingColor;
        }
    }
}
