namespace DeepSleep.Pipeline
{
    using DeepSleep.Auth;
    using DeepSleep.Configuration;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestRoutingPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestRoutingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestRoutingPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var routes = context?.RequestServices?.GetService<IApiRoutingTable>();
            var resolver = context?.RequestServices?.GetService<IUriRouteResolver>();
            var defaultRequestConfig = context?.RequestServices?.GetService<IApiRequestConfiguration>();

            if (await context.ProcessHttpRequestRouting(routes, resolver, defaultRequestConfig).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestRoutingPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request routing.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestRouting(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestRoutingPipelineComponent>();
        }

        /// <summary>Processes the HTTP request routing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="defaultRequestConfig">The default request configuration.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestRouting(this ApiRequestContext context,
            IApiRoutingTable routes,
            IUriRouteResolver resolver,
            IApiRequestConfiguration defaultRequestConfig)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (routes != null && resolver != null)
                {
                    context.Routing.Route = await context.GetRoutingItem(resolver, routes, defaultRequestConfig).ConfigureAwait(false);
                    context.Routing.Template = await context.GetRoutingTemplate(resolver, routes).ConfigureAwait(false);
                }

                context.Configuration = MergeConfigurations(context, defaultRequestConfig, context.Routing?.Route?.Configuration);

                if (context.Routing.Route?.Location?.MethodInfo != null)
                {
                    var attributes = context.Routing.Route.Location.MethodInfo.GetCustomAttributes();

                    // Find any attribute request pipeline components
                    // and add the to the final configuration of the request.
                    attributes
                        .Where(a => a as IRequestPipelineComponent != null)
                        .Select(a => a as IRequestPipelineComponent)
                        .ToList()
                        .ForEach(p => context.Configuration.PipelineComponents.Add(p));

                    // Find any attribute validator components
                    // and add the to the final configuration of the request.
                    attributes
                        .Where(a => a as IEndpointValidatorComponent != null)
                        .Select(a => a as IEndpointValidatorComponent)
                        .ToList()
                        .ForEach(v => context.Configuration.Validators.Add(v));

                    // Find any authentication components
                    // and add the to the final configuration of the request.
                    var authenticationComponents = attributes
                        .Where(a => a as IAuthenticationComponent != null)
                        .Select(a => a as IAuthenticationComponent)
                        .ToList();

                    if (authenticationComponents.Count > 0)
                    {
                        context.Configuration.AuthenticationProviders = authenticationComponents;
                    }

                    // Find any authorization components
                    // and add the to the final configuration of the request.
                    var authorizationComponents = attributes
                        .Where(a => a as IAuthorizationComponent != null)
                        .Select(a => a as IAuthorizationComponent)
                        .ToList();

                    if (authorizationComponents.Count > 0)
                    {
                        context.Configuration.AuthorizationProviders = authorizationComponents;
                    }
                }


                if (context.Configuration?.RequestValidation?.MaxRequestLength != null && context.ConfigureMaxRequestLength != null)
                {
                    try
                    {
                        context.ConfigureMaxRequestLength(context.Configuration.RequestValidation.MaxRequestLength.Value);
                    }
                    catch { }
                }

                return true;
            }

            return false;
        }

        /// <summary>Gets the route information.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        private static async Task<ApiRoutingItem> GetRoutingItem(this ApiRequestContext context,
            IUriRouteResolver resolver,
            IApiRoutingTable routes,
            IApiRequestConfiguration defaultRequestConfiguration)
        {
            // -----------------------------------------------------------------
            // We want to trick the routing engine to treat HEAD requests as GET
            // http://tools.ietf.org/html/rfc7231#section-4.3.2
            // -----------------------------------------------------------------
            ApiRoutingItem routeInfo;

            if (context.Request.Method.In(StringComparison.InvariantCultureIgnoreCase, "HEAD"))
            {
                routeInfo = await resolver.MatchRoute(
                    routes,
                    "HEAD",
                    context.Request.Path).ConfigureAwait(false);

                if (routeInfo == null)
                {
                    routeInfo = await resolver.MatchRoute(
                        routes,
                        "GET",
                        context.Request.Path).ConfigureAwait(false);

                    if (routeInfo != null)
                    {
                        var enableHeadForGetRequests = routeInfo.Configuration?.EnableHeadForGetRequests
                            ?? defaultRequestConfiguration?.EnableHeadForGetRequests
                            ?? ApiRequestContext.GetDefaultRequestConfiguration().EnableHeadForGetRequests
                            ?? true;

                        if (!enableHeadForGetRequests)
                        {
                            routeInfo = null;
                        }
                    }
                }
            }
            else if (context.Request.IsCorsPreflightRequest())
            {
                if (context.Request.CrossOriginRequest.AccessControlRequestMethod.In(StringComparison.InvariantCultureIgnoreCase, "HEAD"))
                {
                    routeInfo = await resolver.MatchRoute(
                        routes,
                        context.Request.CrossOriginRequest.AccessControlRequestMethod,
                        context.Request.Path).ConfigureAwait(false);

                    if (routeInfo == null)
                    {
                        routeInfo = await resolver.MatchRoute(
                            routes,
                            "GET",
                            context.Request.Path).ConfigureAwait(false);

                        if (routeInfo != null)
                        {
                            var enableHeadForGetRequests = routeInfo.Configuration?.EnableHeadForGetRequests
                                ?? defaultRequestConfiguration?.EnableHeadForGetRequests
                                ?? ApiRequestContext.GetDefaultRequestConfiguration().EnableHeadForGetRequests
                                ?? true;

                            if (!enableHeadForGetRequests)
                            {
                                routeInfo = null;
                            }
                        }
                    }
                }
                else
                {
                    routeInfo = await resolver.MatchRoute(
                        routes,
                        context.Request.CrossOriginRequest.AccessControlRequestMethod,
                        context.Request.Path).ConfigureAwait(false);
                }
            }
            else
            {
                routeInfo = await resolver.MatchRoute(
                    routes,
                    context.Request.Method,
                    context.Request.Path).ConfigureAwait(false);
            }

            return routeInfo;
        }

        /// <summary>Gets the template information.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <returns></returns>
        private static async Task<ApiRoutingTemplate> GetRoutingTemplate(
            this ApiRequestContext context,
            IUriRouteResolver resolver,
            IApiRoutingTable routes)
        {
            RouteMatch result;
            ApiRoutingTemplate template = null;

            foreach (var route in routes.GetRoutes())
            {
                result = await resolver.ResolveRoute(route.Template, context.Request.Path).ConfigureAwait(false);

                if (result?.IsMatch ?? false)
                {
                    if (template == null)
                    {
                        template = new ApiRoutingTemplate(route.Template);
                    }

                    template.Locations.Add(new ApiEndpointLocation(
                        controller: route.Location.Controller,
                        endpoint: route.Location.Endpoint,
                        httpMethod: route.Location.HttpMethod,
                        methodInfo: route.Location.MethodInfo,
                        bodyParameterType: route.Location.BodyParameterType,
                        uriParameterType: route.Location.UriParameterType,
                        simpleParameters: route.Location.SimpleParameters,
                        methodReturnType: route.Location.MethodReturnType));
                }
            }

            return template;
        }

        /// <summary>Merges the configurations.</summary>
        /// <param name="context">The context.</param>
        /// <param name="defaultConfig">The default configuration.</param>
        /// <param name="endpointConfig">The endpoint configuration.</param>
        /// <returns></returns>
        private static IApiRequestConfiguration MergeConfigurations(
            ApiRequestContext context,
            IApiRequestConfiguration defaultConfig,
            IApiRequestConfiguration endpointConfig)
        {
            IApiRequestConfiguration systemConfig = ApiRequestContext.GetDefaultRequestConfiguration();

            IApiRequestConfiguration requestConfig = (endpointConfig != null)
                ? GetNewConfiguration(context, endpointConfig)
                : GetNewConfiguration(context, defaultConfig);

            if (requestConfig == null)
            {
                return systemConfig;
            }


            requestConfig.ApiErrorResponseProvider = endpointConfig?.ApiErrorResponseProvider
                ?? defaultConfig?.ApiErrorResponseProvider
                ?? systemConfig.ApiErrorResponseProvider;

            requestConfig.AllowAnonymous = endpointConfig?.AllowAnonymous
                ?? defaultConfig?.AllowAnonymous
                ?? systemConfig.AllowAnonymous;

            requestConfig.Deprecated = endpointConfig?.Deprecated
                ?? defaultConfig?.Deprecated
                ?? systemConfig.Deprecated;

            requestConfig.IncludeRequestIdHeaderInResponse = endpointConfig?.IncludeRequestIdHeaderInResponse
                ?? defaultConfig?.IncludeRequestIdHeaderInResponse
                ?? systemConfig.IncludeRequestIdHeaderInResponse;

            requestConfig.EnableHeadForGetRequests = endpointConfig?.EnableHeadForGetRequests
                ?? defaultConfig?.EnableHeadForGetRequests
                ?? systemConfig.EnableHeadForGetRequests;


            // ----------------------------
            // Authorization Components
            // ----------------------------
            if (endpointConfig?.AuthorizationProviders != null || defaultConfig?.AuthorizationProviders != null)
            {
                if (endpointConfig?.AuthorizationProviders != null)
                {
                    requestConfig.AuthorizationProviders = new List<IAuthorizationComponent>(endpointConfig.AuthorizationProviders);
                }
                else if (defaultConfig?.AuthorizationProviders != null)
                {
                    requestConfig.AuthorizationProviders = new List<IAuthorizationComponent>(defaultConfig.AuthorizationProviders);
                }
                else
                {
                    requestConfig.AuthorizationProviders = systemConfig.AuthorizationProviders;
                }
            }
            else
            {
                requestConfig.AuthorizationProviders = systemConfig.AuthorizationProviders;
            }


            // ----------------------------
            // Authentication Components
            // ----------------------------
            if (endpointConfig?.AuthenticationProviders != null || defaultConfig?.AuthenticationProviders != null)
            {
                if (endpointConfig?.AuthenticationProviders != null)
                {
                    requestConfig.AuthenticationProviders = new List<IAuthenticationComponent>(endpointConfig.AuthenticationProviders);
                }
                else if (defaultConfig?.AuthenticationProviders != null)
                {
                    requestConfig.AuthenticationProviders = new List<IAuthenticationComponent>(defaultConfig.AuthenticationProviders);
                }
                else
                {
                    requestConfig.AuthenticationProviders = systemConfig.AuthenticationProviders;
                }
            }
            else
            {
                requestConfig.AuthenticationProviders = systemConfig.AuthenticationProviders;
            }


            // ----------------------------
            // Validator Components
            // ----------------------------
            if (endpointConfig?.Validators != null || defaultConfig?.Validators != null)
            {
                if (endpointConfig?.Validators != null)
                {
                    requestConfig.Validators = new List<IEndpointValidatorComponent>(endpointConfig.Validators);
                }
                else if (defaultConfig?.Validators != null)
                {
                    requestConfig.Validators = new List<IEndpointValidatorComponent>(defaultConfig.Validators);
                }
                else
                {
                    requestConfig.Validators = systemConfig.Validators;
                }
            }
            else
            {
                requestConfig.Validators = systemConfig.Validators;
            }

            // ----------------------------
            // Pipeline Components
            // ----------------------------
            if (endpointConfig?.PipelineComponents != null || defaultConfig?.PipelineComponents != null)
            {
                if (endpointConfig?.PipelineComponents != null)
                {
                    requestConfig.PipelineComponents = new List<IRequestPipelineComponent>(endpointConfig.PipelineComponents);
                }
                else if (defaultConfig?.PipelineComponents != null)
                {
                    requestConfig.PipelineComponents = new List<IRequestPipelineComponent>(defaultConfig.PipelineComponents);
                }
                else
                {
                    requestConfig.PipelineComponents = systemConfig.PipelineComponents;
                }
            }
            else
            {
                requestConfig.PipelineComponents = systemConfig.PipelineComponents;
            }

            // ----------------------------
            // Language Support Validation
            // ----------------------------
            if (endpointConfig?.LanguageSupport != null || defaultConfig?.LanguageSupport != null)
            {
                requestConfig.LanguageSupport = new ApiLanguageSupportConfiguration
                {
                    FallBackLanguage = endpointConfig?.LanguageSupport?.FallBackLanguage
                        ?? defaultConfig?.LanguageSupport?.FallBackLanguage
                        ?? systemConfig.LanguageSupport?.FallBackLanguage,

                    SupportedLanguages = new List<string>(endpointConfig?.LanguageSupport?.SupportedLanguages
                        ?? defaultConfig?.LanguageSupport?.SupportedLanguages
                        ?? systemConfig.LanguageSupport?.SupportedLanguages),

                    UseAcceptedLanguageAsThreadCulture = endpointConfig?.LanguageSupport?.UseAcceptedLanguageAsThreadCulture
                        ?? defaultConfig?.LanguageSupport?.UseAcceptedLanguageAsThreadCulture
                        ?? systemConfig.LanguageSupport?.UseAcceptedLanguageAsThreadCulture,

                    UseAcceptedLanguageAsThreadUICulture = endpointConfig?.LanguageSupport?.UseAcceptedLanguageAsThreadUICulture
                        ?? defaultConfig?.LanguageSupport?.UseAcceptedLanguageAsThreadUICulture
                        ?? systemConfig.LanguageSupport?.UseAcceptedLanguageAsThreadUICulture
                };

                var endpointSupportedLanguages = endpointConfig?.LanguageSupport?.SupportedLanguages != null
                    ? new List<string>(endpointConfig.LanguageSupport.SupportedLanguages)
                    : null;

                var defaultSupportedLanguages = defaultConfig?.LanguageSupport?.SupportedLanguages != null
                    ? new List<string>(defaultConfig.LanguageSupport.SupportedLanguages)
                    : null;

                requestConfig.LanguageSupport.SupportedLanguages = endpointSupportedLanguages ?? defaultSupportedLanguages ?? systemConfig.LanguageSupport?.SupportedLanguages;
            }
            else
            {
                requestConfig.LanguageSupport = systemConfig.LanguageSupport;
            }

            // ----------------------------
            // Merge Request Validation
            // ----------------------------
            if (endpointConfig?.RequestValidation != null || defaultConfig?.RequestValidation != null)
            {
                requestConfig.RequestValidation = new ApiRequestValidationConfiguration
                {
                    MaxHeaderLength = endpointConfig?.RequestValidation?.MaxHeaderLength
                        ?? defaultConfig?.RequestValidation?.MaxHeaderLength
                        ?? systemConfig.RequestValidation?.MaxHeaderLength,
                    MaxRequestUriLength = endpointConfig?.RequestValidation?.MaxRequestUriLength
                        ?? defaultConfig?.RequestValidation?.MaxRequestUriLength
                        ?? systemConfig.RequestValidation?.MaxRequestUriLength,
                    MaxRequestLength = endpointConfig?.RequestValidation?.MaxRequestLength
                        ?? defaultConfig?.RequestValidation?.MaxRequestLength
                        ?? systemConfig.RequestValidation?.MaxRequestLength,
                    AllowRequestBodyWhenNoModelDefined = endpointConfig?.RequestValidation?.AllowRequestBodyWhenNoModelDefined
                        ?? defaultConfig?.RequestValidation?.AllowRequestBodyWhenNoModelDefined
                        ?? systemConfig.RequestValidation?.AllowRequestBodyWhenNoModelDefined,
                    RequireContentLengthOnRequestBodyRequests = endpointConfig?.RequestValidation?.RequireContentLengthOnRequestBodyRequests
                        ?? defaultConfig?.RequestValidation?.RequireContentLengthOnRequestBodyRequests
                        ?? systemConfig.RequestValidation?.RequireContentLengthOnRequestBodyRequests
                };
            }
            else
            {
                requestConfig.RequestValidation = systemConfig.RequestValidation;
            }

            // ----------------------------
            // Merge Cache Directive
            // ----------------------------
            if (endpointConfig?.CacheDirective != null || defaultConfig?.CacheDirective != null)
            {
                requestConfig.CacheDirective = new ApiCacheDirectiveConfiguration
                {
                    Cacheability = endpointConfig?.CacheDirective?.Cacheability
                        ?? defaultConfig?.CacheDirective?.Cacheability
                        ?? systemConfig.CacheDirective?.Cacheability,

                    CacheLocation = endpointConfig?.CacheDirective?.CacheLocation
                        ?? defaultConfig?.CacheDirective?.CacheLocation
                        ?? systemConfig.CacheDirective?.CacheLocation,

                    ExpirationSeconds = endpointConfig?.CacheDirective?.ExpirationSeconds
                        ?? defaultConfig?.CacheDirective?.ExpirationSeconds
                        ?? systemConfig.CacheDirective?.ExpirationSeconds,

                    VaryHeaderValue = endpointConfig?.CacheDirective?.VaryHeaderValue
                        ?? defaultConfig?.CacheDirective?.VaryHeaderValue
                        ?? systemConfig.CacheDirective?.VaryHeaderValue
                };
            }
            else
            {
                requestConfig.CacheDirective = systemConfig.CacheDirective;
            }


            // ----------------------------
            // Merge Cross Origin Configuration
            // ----------------------------
            if (endpointConfig?.CrossOriginConfig != null || defaultConfig?.CrossOriginConfig != null)
            {
                requestConfig.CrossOriginConfig = new ApiCrossOriginConfiguration
                {
                    AllowCredentials = endpointConfig?.CrossOriginConfig?.AllowCredentials
                        ?? defaultConfig?.CrossOriginConfig?.AllowCredentials
                        ?? systemConfig?.CrossOriginConfig?.AllowCredentials,
                    MaxAgeSeconds = endpointConfig?.CrossOriginConfig?.MaxAgeSeconds
                        ?? defaultConfig?.CrossOriginConfig?.MaxAgeSeconds
                        ?? systemConfig?.CrossOriginConfig?.MaxAgeSeconds
                };

                var endpointAllowedOrigins = endpointConfig?.CrossOriginConfig?.AllowedOrigins != null
                    ? new List<string>(endpointConfig.CrossOriginConfig.AllowedOrigins)
                    : null;

                var defaultAllowedOrigins = defaultConfig?.CrossOriginConfig?.AllowedOrigins != null
                    ? new List<string>(defaultConfig.CrossOriginConfig.AllowedOrigins)
                    : null;

                requestConfig.CrossOriginConfig.AllowedOrigins = endpointAllowedOrigins ?? defaultAllowedOrigins ?? systemConfig.CrossOriginConfig?.AllowedOrigins;

                var endpointExposeHeaders = endpointConfig?.CrossOriginConfig?.ExposeHeaders != null
                    ? new List<string>(endpointConfig.CrossOriginConfig.ExposeHeaders)
                    : null;

                var defaultExposeHeaders = defaultConfig?.CrossOriginConfig?.ExposeHeaders != null
                    ? new List<string>(defaultConfig.CrossOriginConfig.ExposeHeaders)
                    : null;

                requestConfig.CrossOriginConfig.ExposeHeaders = endpointExposeHeaders ?? defaultExposeHeaders ?? systemConfig.CrossOriginConfig?.ExposeHeaders;

                var endpointAllowedHeaders = endpointConfig?.CrossOriginConfig?.AllowedHeaders != null
                    ? new List<string>(endpointConfig.CrossOriginConfig.AllowedHeaders)
                    : null;

                var defaultAllowedHeaders = defaultConfig?.CrossOriginConfig?.AllowedHeaders != null
                    ? new List<string>(defaultConfig.CrossOriginConfig.AllowedHeaders)
                    : null;

                requestConfig.CrossOriginConfig.AllowedHeaders = endpointAllowedHeaders ?? defaultAllowedHeaders ?? systemConfig.CrossOriginConfig?.AllowedHeaders;

            }
            else
            {
                requestConfig.CrossOriginConfig = systemConfig.CrossOriginConfig;
            }


            // ----------------------------
            // Merge Read Write Configuration
            // ----------------------------
            if (endpointConfig?.ReadWriteConfiguration != null || defaultConfig?.ReadWriteConfiguration != null)
            {
                requestConfig.ReadWriteConfiguration = new ApiReadWriteConfiguration
                {
                    ReaderResolver = endpointConfig?.ReadWriteConfiguration?.ReaderResolver
                        ?? defaultConfig?.ReadWriteConfiguration?.ReaderResolver
                        ?? systemConfig.ReadWriteConfiguration?.ReaderResolver,

                    WriterResolver = endpointConfig?.ReadWriteConfiguration?.WriterResolver
                        ?? defaultConfig?.ReadWriteConfiguration?.WriterResolver
                        ?? systemConfig.ReadWriteConfiguration?.WriterResolver,

                    AcceptHeaderOverride = endpointConfig?.ReadWriteConfiguration?.AcceptHeaderOverride
                        ?? defaultConfig?.ReadWriteConfiguration?.AcceptHeaderOverride
                        ?? systemConfig.ReadWriteConfiguration?.AcceptHeaderOverride,
                };

                var endpointReadableMediaTypes = endpointConfig?.ReadWriteConfiguration?.ReadableMediaTypes != null
                    ? new List<string>(endpointConfig.ReadWriteConfiguration.ReadableMediaTypes)
                    : null;

                var defaultReadableMediaTypes = defaultConfig?.ReadWriteConfiguration?.ReadableMediaTypes != null
                    ? new List<string>(defaultConfig.ReadWriteConfiguration.ReadableMediaTypes)
                    : null;

                var endpointWriteableMediaTypes = endpointConfig?.ReadWriteConfiguration?.WriteableMediaTypes != null
                    ? new List<string>(endpointConfig.ReadWriteConfiguration.WriteableMediaTypes)
                    : null;

                var defaultWriteableMediaTypes = defaultConfig?.ReadWriteConfiguration?.WriteableMediaTypes != null
                    ? new List<string>(defaultConfig.ReadWriteConfiguration.WriteableMediaTypes)
                    : null;

                requestConfig.ReadWriteConfiguration.ReadableMediaTypes = endpointReadableMediaTypes
                        ?? defaultReadableMediaTypes
                        ?? systemConfig.ReadWriteConfiguration?.ReadableMediaTypes;

                requestConfig.ReadWriteConfiguration.WriteableMediaTypes = endpointWriteableMediaTypes
                        ?? defaultWriteableMediaTypes
                        ?? systemConfig.ReadWriteConfiguration?.WriteableMediaTypes;
            }
            else
            {
                requestConfig.ReadWriteConfiguration = systemConfig.ReadWriteConfiguration;
            }


            // ------------------------------------
            // Merge Validation Error Configuration
            // ------------------------------------
            if (endpointConfig?.ValidationErrorConfiguration != null || defaultConfig?.ValidationErrorConfiguration != null)
            {
                requestConfig.ValidationErrorConfiguration = new ApiValidationErrorConfiguration
                {
                    UriBindingError = endpointConfig?.ValidationErrorConfiguration?.UriBindingError
                        ?? defaultConfig?.ValidationErrorConfiguration?.UriBindingError
                        ?? systemConfig.ValidationErrorConfiguration?.UriBindingError,

                    UriBindingValueError = endpointConfig?.ValidationErrorConfiguration?.UriBindingValueError
                        ?? defaultConfig?.ValidationErrorConfiguration?.UriBindingValueError
                        ?? systemConfig.ValidationErrorConfiguration?.UriBindingValueError,

                    RequestDeserializationError = endpointConfig?.ValidationErrorConfiguration?.RequestDeserializationError
                        ?? defaultConfig?.ValidationErrorConfiguration?.RequestDeserializationError
                        ?? systemConfig.ValidationErrorConfiguration?.RequestDeserializationError,

                    HttpStatusMode = endpointConfig?.ValidationErrorConfiguration?.HttpStatusMode
                        ?? defaultConfig?.ValidationErrorConfiguration?.HttpStatusMode
                        ?? systemConfig.ValidationErrorConfiguration.HttpStatusMode
                };
            }
            else
            {
                requestConfig.ValidationErrorConfiguration = systemConfig.ValidationErrorConfiguration;
            }


            return requestConfig;
        }

        /// <summary>Resolves the configuration.</summary>
        /// <param name="context">The context.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        private static IApiRequestConfiguration GetNewConfiguration(ApiRequestContext context, IApiRequestConfiguration config)
        {
            if (config == null)
            {
                return null;
            }

            IApiRequestConfiguration init = null;

            if (context?.RequestServices != null)
            {
                try
                {
                    init = context.RequestServices.GetService(config.GetType()) as IApiRequestConfiguration;
                }
                catch { }
            }

            if (init == null)
            {
                init = Activator.CreateInstance(config.GetType()) as IApiRequestConfiguration;
            }

            return init;
        }
    }
}
