namespace DeepSleep.OpenApi
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Media;
    using DeepSleep.OpenApi.Controllers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public static class OpenApiCoreHttpExtensionMethods
    {
        /// <summary>Uses the deep sleep open API.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configure">The configure.</param>
        /// <returns></returns>
        public static IServiceCollection UseDeepSleepOpenApi(
            this IServiceCollection services,
            Action<IDeepSleepOasConfigurationProvider> configure = null)
        {
            var configuration = new DeepSleepOasConfigurationProvider
            {
                Info = new OpenApiInfo
                {
                    Version = "1.0",
                    Title = Assembly.GetEntryAssembly().GetName().Name
                },
                PrefixNamesWithNamespace = false,
                V2RouteTemplate = "openapi/v2",
                V3RouteTemplate = "openapi/v3",
                XmlDocumentationFileNames = new List<string>(),
                EnumModeling = OasEnumModeling.AsString,
                NamingPolicy = null,
                EnumNamingPolicy = null
            };

            if (configure != null)
            {
                configure(configuration);
            }

            configuration.Info = configuration.Info ?? new OpenApiInfo
            {
                Version = "1.0",
                Title = Assembly.GetEntryAssembly().GetName().Name
            };

            configuration.Info.Version = configuration.Info.Version ?? "1.0";
            configuration.Info.Title = configuration.Info.Title ?? Assembly.GetEntryAssembly().GetName().Name;
            configuration.XmlDocumentationFileNames = configuration.XmlDocumentationFileNames ?? new List<string>();

            Func<string, string, DeepSleepRouteRegistration> getRoute = (string endpoint, string template) =>
            {
                return new DeepSleepRouteRegistration(
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
                    });
            };

            if (!string.IsNullOrWhiteSpace(configuration.V2RouteTemplate))
            {
                services.AddTransient<IDeepSleepSingleRouteRegistrationProvider, OasV2RouteRegistrationProvider>((p) =>
                {
                    return new OasV2RouteRegistrationProvider(getRoute(nameof(OasController.DocV2), configuration.V2RouteTemplate));
                });
            }

            if (!string.IsNullOrWhiteSpace(configuration.V3RouteTemplate))
            {
                services.AddTransient<IDeepSleepSingleRouteRegistrationProvider, OasV2RouteRegistrationProvider>((p) =>
                {
                    return new OasV2RouteRegistrationProvider(getRoute(nameof(OasController.DocV3), configuration.V3RouteTemplate));
                });
            }

            return services
                .AddSingleton<IDeepSleepOasConfigurationProvider>(configuration)
                .AddScoped<IDeepSleepOasGenerator, DeepSleepOasGenerator>()
                .AddScoped<DeepSleepOasJsonFormatter>()
                .AddScoped<DeepSleepOasYamlFormatter>()
                .AddTransient<OasController>();
        }
    }
}
