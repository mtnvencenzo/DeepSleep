namespace DeepSleep.NetCore
{
    using DeepSleep.Formatting;
    using DeepSleep.Auth;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using DeepSleep.Validation;
    using System;
    using DeepSleep.Pipeline;
    using DeepSleep.Formatting.Formatters;
    using DeepSleep.Auth.Providers;
    using DeepSleep.NetCore.Controllers;

    /// <summary>
    /// 
    /// </summary>
    public static class ApiCoreHttpExtensionMethods
    {
        #region Helper Methods

        /// <summary>Gets the default validation provider.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiValidationProvider __getDefaultValidationProvider(IServiceProvider serviceProvider)
        {
            return new DefaultApiValidationProvider(serviceProvider)
                .RegisterInvoker<TypeBasedValidationInvoker>();
        }

        /// <summary>Gets the default response message processor provider.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiResponseMessageProcessorProvider __getDefaultResponseMessageProcessorProvider(IServiceProvider serviceProvider)
        {
            var provider = new DefaultApiResponseMessageProcessorProvider(serviceProvider)
                .RegisterProcessor<ApiResultResponseMessageProcessor>();

            return provider;
        }

        /// <summary>Gets the default formatter factory.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IFormatStreamReaderWriterFactory __getDefaultFormatterFactory(IServiceProvider serviceProvider)
        {
            return new HttpMediaTypeStreamWriterFactory()
                .Add(new JsonHttpFormatter(), new string[] { "application/json", "text/json", "application/json-patch+json" }, new string[] { "utf-32, utf-16, utf-8" })
                .Add(new XmlHttpFormatter(), new string[] { "text/xml", "application/xml" }, new string[] { "utf-32, utf-16, utf-8" });
        }

        /// <summary>Gets the default authentication factory.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IAuthenticationFactory __getDefaultAuthenticationFactory(IServiceProvider serviceProvider)
        {
            return new DefaultAuthenticationFactory()
                .AddProvider(new HMACSHA256SharedKeyAuthenticationProvider("api"));
        }

        /// <summary>Gets the default cross origin configuration resolver.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static ICrossOriginConfigResolver __getDefaultCrossOriginConfigResolver(IServiceProvider serviceProvider)
        {
            return new DefaultCrossOriginConfigResolver()
                .AddExposeHeaders(new string[]
                {
                    "X-CorrelationId",
                    "X-Deprecated",
                    "X-Api-Message",
                    "X-Api-Version",
                    "X-Api-RequestId",
                    "X-Allow-Accept",
                    "X-Allow-Accept-Charset",
                    "X-PrettyPrint",
                    "Location"
                })
                .AddAllowedOrigins(new string[] { "*" });
        }

        /// <summary>Gets the default request pipeline.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiRequestPipeline __getDefaultRequestPipeline(IServiceProvider serviceProvider)
        {
            return new DefaultApiRequestPipelineBuilder()
                .Build();
        }

        /// <summary>Gets the default API response message converter.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        private static IApiResponseMessageConverter __getDefaultApiResponseMessageConverter(IServiceProvider serviceProvider)
        {
            return new DefaultApiResponseMessageConverter();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        private static IApiRoutingTable __getDefaultRoutingTable(IServiceProvider serviceProvider, IApiServiceConfiguration config)
        {
            var table = new DefaultApiRoutingTable();

            if (config?.UsePingEndpoint ?? false)
            {
                table.AddRoute(
                   template: $"ping",
                   httpMethod: "GET",
                   name: $"GET_ping",
                   controller: typeof(PingController),
                   endpoint: nameof(PingController.Ping),
                   config: new ApiResourceConfig
                   {
                       AllowAnonymous = true,
                       CacheDirective = new HttpCacheDirective
                       {
                           Cacheability = HttpCacheType.NoCache,
                           CacheLocation = HttpCacheLocation.Private,
                           ExpirationSeconds = -5
                       },
                       Deprecated = false,
                       ResourceId = $"{Guid.Empty}_Ping"
                   }).ConfigureAwait(false).GetAwaiter();
            }

            if (config?.UseEnvironmentEndpoint ?? false)
            {
                table.AddRoute(
                   template: $"env",
                   httpMethod: "GET",
                   name: $"GET_env",
                   controller: typeof(EnnvironmentController),
                   endpoint: nameof(EnnvironmentController.Env),
                   config: new ApiResourceConfig
                   {
                       AllowAnonymous = true,
                       CacheDirective = new HttpCacheDirective
                       {
                           Cacheability = HttpCacheType.NoCache,
                           CacheLocation = HttpCacheLocation.Private,
                           ExpirationSeconds = -5
                       },
                       Deprecated = false,
                       ResourceId = $"{Guid.Empty}_Environment"
                   }).ConfigureAwait(false).GetAwaiter();
            }

            return table;
        }

        #endregion

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
            return services
                .AddScoped<IApiRequestContextResolver, DefaultApiRequestContextResolver>()
                .AddScoped<IUriRouteResolver, DefaultRouteResolver>()
                .AddScoped<IAuthenticationFactory, IAuthenticationFactory>((p) => config.AuthenticationFactoryResolver(p, __getDefaultAuthenticationFactory(p)))
                .AddScoped<IFormatStreamReaderWriterFactory, IFormatStreamReaderWriterFactory>((p) => config.FormatFactoryResolver(p, __getDefaultFormatterFactory(p)))
                .AddScoped<ICrossOriginConfigResolver, ICrossOriginConfigResolver>((p) => config.CrossOriginConfigResolver(p, __getDefaultCrossOriginConfigResolver(p)))
                .AddScoped<IApiValidationProvider, IApiValidationProvider>((p) => config.ApiValidationProvider(p, __getDefaultValidationProvider(p)))
                .AddScoped<IApiResponseMessageProcessorProvider, IApiResponseMessageProcessorProvider>((p) => config.ApiResponseMessageProcessorProvider(p, __getDefaultResponseMessageProcessorProvider(p)))
                .AddScoped<IApiResponseMessageConverter, IApiResponseMessageConverter>((p) => config.ApiResponseMessageConverter(p, __getDefaultApiResponseMessageConverter(p)))
                .AddSingleton<IApiRoutingTable, IApiRoutingTable>((p) => config.RoutingTableResolver(p, __getDefaultRoutingTable(p, config)))
                .AddSingleton<IApiRequestPipeline, IApiRequestPipeline>((p) => config.ApiRequestPipelineBuilder(p, __getDefaultRequestPipeline(p)))
                .AddSingleton<IApiServiceConfiguration, IApiServiceConfiguration>((p) => config);
        }
    }
}
