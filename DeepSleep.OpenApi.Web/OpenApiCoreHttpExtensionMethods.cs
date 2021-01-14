namespace DeepSleep.OpenApi.Web
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.OpenApi.Web.Controllers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 
    /// </summary>
    public static class OpenApiCoreHttpExtensionMethods
    {
        /// <summary>Uses the open API endpoint.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="routeTemplate">The route template.</param>
        /// <param name="prefixNamesWithNamespace">if set to <c>true</c> [prefix names with namespace].</param>
        /// <param name="includeHeadOperationsForGets">if set to <c>true</c> [include head operations for gets].</param>
        /// <returns></returns>
        public static IApplicationBuilder UseOpenApiEndpoint(this IApplicationBuilder builder,
            string routeTemplate = "openapi",
            bool prefixNamesWithNamespace = false,
            bool includeHeadOperationsForGets = true)
        {
            DefaultOpenApiGenerator.PrefixNamesWithNamespace = prefixNamesWithNamespace;
            DefaultOpenApiGenerator.IncludeHeadOperationsForGets = includeHeadOperationsForGets;

            var table = builder.ApplicationServices.GetRequiredService<IApiRoutingTable>();

            table.AddRoute(new ApiRouteRegistration(
                template: routeTemplate,
                httpMethods: new[] { "GET" },
                controller: Type.GetType(typeof(OpenApiController).AssemblyQualifiedName),
                endpoint: nameof(OpenApiController.Doc),
                config: new DefaultApiRequestConfiguration
                {
                    AllowAnonymous = true,
                    ReadWriteConfiguration = new ApiReadWriteConfiguration
                    {
                        AcceptHeaderOverride = "application/json; q=1.0, test/json; q=0.9",
                        WriteableMediaTypes = new List<string> { "application/json", "test/json" }
                    }
                }));

            return builder;
        }

        /// <summary>Uses the open API services.</summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection UseOpenApiServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IOpenApiGenerator, DefaultOpenApiGenerator>()
                .AddTransient<OpenApiController>();
        }
    }
}
