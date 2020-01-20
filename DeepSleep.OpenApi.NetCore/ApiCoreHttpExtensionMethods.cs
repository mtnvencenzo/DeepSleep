namespace DeepSleep.OpenApi.NetCore
{
    using DeepSleep.Configuration;
    using DeepSleep.OpenApi.NetCore.Controllers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System;


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

            table.AddRoute(
               template: routeTemplate,
               httpMethod: "GET",
               name: $"GET_openapi",
               controller: typeof(OpenApiController),
               endpoint: nameof(OpenApiController.Doc),
               config: new DefaultApiRequestConfiguration
               {
                   AllowAnonymous = true,
                   CacheDirective = new HttpCacheDirective
                   {
                       Cacheability = HttpCacheType.NoCache,
                       CacheLocation = HttpCacheLocation.Private,
                       ExpirationSeconds = -5
                   },
                   Deprecated = false,
                   ResourceId = $"{Guid.Empty}_OpenApi"
               });

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
