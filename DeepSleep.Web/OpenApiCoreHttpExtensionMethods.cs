namespace DeepSleep.Web
{
    using DeepSleep.OpenApi;
    using DeepSleep.Web.Controllers;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json;

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

            services.AddSingleton<IDeepSleepOasConfigurationProvider>(configuration);

            return services
                .AddScoped<IDeepSleepOasGenerator, DeepSleepOasGenerator>()
                .AddScoped<DeepSleepOasJsonFormatter>()
                .AddScoped<DeepSleepOasYamlFormatter>()
                .AddTransient<OasController>();
        }
    }
}
