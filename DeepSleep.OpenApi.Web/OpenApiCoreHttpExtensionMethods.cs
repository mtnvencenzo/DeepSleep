namespace DeepSleep.OpenApi.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Formatting;
    using DeepSleep.OpenApi.Web.Controllers;
    using Microsoft.AspNetCore.Builder;
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
        /// <summary>Uses the open API endpoint.</summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseOpenApiEndpoint(this IApplicationBuilder builder)
        {
            var table = builder.ApplicationServices.GetRequiredService<IApiRoutingTable>();
            var configuration = builder.ApplicationServices.GetRequiredService<IOpenApiConfigurationProvider>();

            Action<string, string> add = (string endpoint, string template) =>
            {
                table.AddRoute(new ApiRouteRegistration(
                    template: template,
                    httpMethods: new[] { "GET" },
                    controller: Type.GetType(typeof(OpenApiController).AssemblyQualifiedName),
                    endpoint: endpoint,
                    config: new DefaultApiRequestConfiguration
                    {
                        AllowAnonymous = true,
                        ReadWriteConfiguration = new ApiReadWriteConfiguration
                        {
                            AcceptHeaderFallback = "application/json; q=1.0, application/yaml; q=0.9",
                            WriterResolver = (resolver) =>
                            {
                                var context = resolver.GetContext();
                                var jsonFormatter = context.RequestServices.GetService<OpenApiJsonFormatter>();
                                var yamlFormatter = context.RequestServices.GetService<OpenApiYamlFormatter>();

                                return Task.FromResult(new FormatterWriteOverrides(new List<IFormatStreamReaderWriter>
                                {
                                    jsonFormatter,
                                    yamlFormatter
                                }));
                            }
                        }
                    }));
            };

            if (!string.IsNullOrWhiteSpace(configuration.V2RouteTemplate))
            {
                add(nameof(OpenApiController.DocV2), configuration.V2RouteTemplate);
            }

            if (!string.IsNullOrWhiteSpace(configuration.V3RouteTemplate))
            {
                add(nameof(OpenApiController.DocV3), configuration.V3RouteTemplate);
            }

            return builder;
        }

        /// <summary>Uses the open API services.</summary>
        /// <param name="services">The services.</param>
        /// <param name="info">The information.</param>
        /// <param name="xmlDocumentationFileNames">The XML documentation file names.</param>
        /// <param name="v2RouteTemplate">The v2 route template.</param>
        /// <param name="v3RouteTemplate">The v3 route template.</param>
        /// <param name="prefixNamesWithNamespace">if set to <c>true</c> [prefix names with namespace].</param>
        /// <param name="includeHeadOperationsForGets">if set to <c>true</c> [include head operations for gets].</param>
        /// <returns></returns>
        public static IServiceCollection UseOpenApiServices(
            this IServiceCollection services, 
            OpenApiInfo info,
            IList<string> xmlDocumentationFileNames = null,
            string v2RouteTemplate = "openapi/v2",
            string v3RouteTemplate = "openapi/v3",
            bool prefixNamesWithNamespace = false,
            bool includeHeadOperationsForGets = true)
        {
            info = info ?? new OpenApiInfo
            {
                Version = "1.0.0.0",
                Title = Assembly.GetEntryAssembly().GetName().Name
            };

            info.Version = info.Version ?? "1.0.0.0";
            info.Title = info.Title ?? Assembly.GetEntryAssembly().GetName().Name;

            services.AddSingleton<IOpenApiConfigurationProvider>(new OpenApiConfigurationProvider
            {
                Info = info,
                IncludeHeadOperationsForGets = includeHeadOperationsForGets,
                PrefixNamesWithNamespace = prefixNamesWithNamespace,
                V2RouteTemplate = v2RouteTemplate,
                V3RouteTemplate = v3RouteTemplate,
                XmlDocumentationFileNames = xmlDocumentationFileNames ?? new List<string>()
            });

            return services
                .AddScoped<IOpenApiGenerator, OpenApiGenerator>()
                .AddScoped<OpenApiJsonFormatter>()
                .AddScoped<OpenApiYamlFormatter>()
                .AddTransient<OpenApiController>();
        }
    }
}
