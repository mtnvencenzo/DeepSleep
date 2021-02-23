namespace DeepSleep.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Media;
    using DeepSleep.Media.Converters;
    using DeepSleep.Media.Serializers;
    using DeepSleep.Health;
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
            var defaultRequetConfiguration = builder.ApplicationServices.GetService<IDeepSleepRequestConfiguration>();

            DiscoverRoutes(builder, routingTable, config);


            // ---------------------------------
            // Setup endpoints registered suring startup
            // ---------------------------------
            IEnumerable<IDeepSleepSingleRouteRegistrationProvider> singleRouteProviders = new List<IDeepSleepSingleRouteRegistrationProvider>();

            try
            {
                singleRouteProviders = builder.ApplicationServices.GetServices<IDeepSleepSingleRouteRegistrationProvider>();
            }
            catch { }

            if (singleRouteProviders != null)
            {
                foreach (var singleRouteProvider in singleRouteProviders)
                {
                    routingTable.AddRoute(singleRouteProvider.GetRouteRegistration());
                }
            }

            // ---------------------------------
            // Setup Console header if being used
            // ---------------------------------
            if (config?.WriteConsoleHeader ?? true)
            {
                try
                {
                    WriteDeepSleepToConsole(routingTable, defaultRequetConfiguration);
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
            var configuration = new DeepSleepServiceConfiguration
            {
                DefaultRequestConfiguration = ApiRequestContext.GetDefaultRequestConfiguration(),
                ExcludePaths = new List<string>(),
                IncludePaths = new List<string> { IncludePaths.All() },
                WriteConsoleHeader = true,
                DiscoveryStrategies = new List<IDeepSleepDiscoveryStrategy>()
            };

            if (configure != null)
            {
                configure(configuration);
            }

            configuration.DefaultRequestConfiguration = configuration.DefaultRequestConfiguration ?? ApiRequestContext.GetDefaultRequestConfiguration();
            configuration.ExcludePaths = configuration.ExcludePaths ?? new List<string>();
            configuration.IncludePaths = configuration.IncludePaths ?? new List<string>();

            var routingTable = new ApiRoutingTable(routePrefix: configuration.RoutePrefix);

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

        /// <summary>Writes the deepsleep to console.</summary>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="defaultConfiguration">The default configuration.</param>
        internal static void WriteDeepSleepToConsole(IApiRoutingTable routingTable, IDeepSleepRequestConfiguration defaultConfiguration)
        {
            var systemConfiguration = ApiRequestContext.GetDefaultRequestConfiguration();
            var existingColor = Console.ForegroundColor;

            var deepSleepAssemblyInfo = GetAssemplyInfo(typeof(ApiRequestContext));
            var deepSleepWebAssemblyInfo = GetAssemplyInfo(typeof(ApiCoreHttpExtensionMethods));
            var deepSleepOpenApiAssemblyInfo = GetAssemplyInfo(Type.GetType("DeepSleep.OpenApi.DeepSleepOasGenerator, deepsleep.openapi", false, false));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($@"");
            Console.WriteLine($@" ____                  _             ");
            Console.WriteLine($@"|    \ ___ ___ ___ ___| |___ ___ ___ ");
            Console.WriteLine($@"|  |  | -_| -_| . |_ -| | -_| -_| . |");
            Console.WriteLine($@"|____/|___|___|  _|___|_|___|___|  _|");
            Console.Write($@"              |_|               |_|  ");
            Console.ForegroundColor = existingColor;

            Console.WriteLine($"   v{deepSleepAssemblyInfo.version}");
            Console.WriteLine($"");

            if (!string.IsNullOrWhiteSpace(deepSleepWebAssemblyInfo.framework))
            {
                Console.WriteLine($"");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Target(s): ");
                Console.ForegroundColor = existingColor;
                Console.WriteLine($"------------------------------------------------");
                Console.ForegroundColor = existingColor;
                Console.WriteLine($"           {deepSleepAssemblyInfo.name}, {deepSleepAssemblyInfo.version}, {deepSleepAssemblyInfo.framework}");

                if (!string.IsNullOrWhiteSpace(deepSleepOpenApiAssemblyInfo.name))
                {
                    Console.WriteLine($"           {deepSleepOpenApiAssemblyInfo.name}, {deepSleepOpenApiAssemblyInfo.version}, {deepSleepOpenApiAssemblyInfo.framework}");
                }

                Console.WriteLine($"           {deepSleepWebAssemblyInfo.name}, {deepSleepWebAssemblyInfo.version}, {deepSleepWebAssemblyInfo.framework}");
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

            Action<string, string, bool> writeRoute = (string method, string template, bool isAutoHead) =>
            {
                existingColor = Console.ForegroundColor;

                Console.ForegroundColor = isAutoHead ? ConsoleColor.Gray : ConsoleColor.Yellow;
                Console.Write($"  {method.ToUpper(),-9}");
                Console.ForegroundColor = existingColor;

                WriteEndpointTemplate(template, isAutoHead);
            };

            routes.ForEach(r =>
            {
                writeRoute(r.HttpMethod, r.Template, false);

                if (string.Compare(r.HttpMethod, "GET", true) == 0)
                {
                    var enableGet = r.Configuration?.EnableHeadForGetRequests
                        ?? defaultConfiguration?.EnableHeadForGetRequests
                        ?? systemConfiguration?.EnableHeadForGetRequests
                        ?? true;

                    if (enableGet)
                    {
                        var matchingHead = routes
                            .Where(m => m.HttpMethod.ToLowerInvariant() == "head")
                            .Where(m => m.Template.ToLowerInvariant() == r.Template.ToLowerInvariant())
                            .FirstOrDefault();

                        if (matchingHead == null)
                        {
                            writeRoute("HEAD", r.Template, true);
                        }
                    }
                }
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
        /// <param name="isAutoHead">if set to <c>true</c> [is automatic head].</param>
        private static void WriteEndpointTemplate(string template, bool isAutoHead)
        {
            var existingColor = Console.ForegroundColor;

            foreach (var c in template.ToCharArray())
            {
                if (c == '{')
                {
                    Console.ForegroundColor = isAutoHead ? ConsoleColor.Gray : ConsoleColor.DarkYellow;
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
            settings.Converters.Add(new JsonStringEnumConverter(namingPolicy: new DefaultNamingPolicy(), allowIntegerValues: true));
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

        /// <summary>Gets the assemply information.</summary>
        /// <param name="typeInAssemply">The type in assemply.</param>
        /// <returns></returns>
        internal static (string name, Version version, string framework) GetAssemplyInfo(Type typeInAssemply)
        {
            try
            {
                if (typeInAssemply != null)
                {
                    var assembly = Assembly.GetAssembly(typeInAssemply);

                    if (assembly != null)
                    {
                        var targetFrameworkAttribute = assembly
                            .GetCustomAttributes(true)
                            .OfType<TargetFrameworkAttribute>()
                            .FirstOrDefault();

                        var targetDisplay = !string.IsNullOrWhiteSpace(targetFrameworkAttribute?.FrameworkName)
                            ? targetFrameworkAttribute.FrameworkName
                            : targetFrameworkAttribute?.FrameworkDisplayName;

                        var assemblyName = assembly.GetName();

                        return (
                            name: assemblyName.Name,
                            version: assemblyName.Version,
                            framework: targetDisplay
                        );
                    }
                }
            }
            catch { }

            return default;
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
