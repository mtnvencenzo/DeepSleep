namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestRoutingPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestRoutingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestRoutingPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="defaultRequestConfig">The default request configuration.</param>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiRoutingTable routes, IUriRouteResolver resolver, IApiRequestConfiguration defaultRequestConfig)
        {
            var context = contextResolver.GetContext();

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
        public static async Task<bool> ProcessHttpRequestRouting(this ApiRequestContext context, IApiRoutingTable routes, IUriRouteResolver resolver, IApiRequestConfiguration defaultRequestConfig)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (routes != null && resolver != null)
                {
                    context.RouteInfo.RoutingItem = await context.GetRouteInfo(resolver, routes, defaultRequestConfig).ConfigureAwait(false);
                    context.RouteInfo.TemplateInfo = await context.GetTemplateInfo(resolver, routes).ConfigureAwait(false);
                }

                return true;
            }

            return false;
        }

        /// <summary>Gets the route information.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="defaultRequestConfig">The default request configuration.</param>
        /// <returns></returns>
        private static async Task<ApiRoutingItem> GetRouteInfo(this ApiRequestContext context, IUriRouteResolver resolver, IApiRoutingTable routes, IApiRequestConfiguration defaultRequestConfig)
        {
            // -----------------------------------------------------------------
            // We want to trick the routing engine to treat HEAD requests as GET
            // http://tools.ietf.org/html/rfc7231#section-4.3.2
            // -----------------------------------------------------------------
            var routeMatch = new Func<string, Task<ApiRoutingItem>>(async (method) =>
            {
                var potentialRoutes = routes.GetRoutes()
                    .Where(r => r.EndpointLocation != null)
                    .Where(r => string.Compare(r.HttpMethod, method, true) == 0);

                RouteMatch result;
                foreach (var route in potentialRoutes)
                {
                    result = await resolver.ResolveRoute(route.Template, context.RequestInfo.Path).ConfigureAwait(false);

                    if (result?.IsMatch ?? false)
                    {
                        var newRoute = CloneRoutingItem(route);
                        newRoute.RouteVariables = result.RouteVariables;
                        return newRoute;
                    }
                }

                return null;
            });

            ApiRoutingItem routeInfo = null;
            if (context.RequestInfo.Method.In(StringComparison.InvariantCultureIgnoreCase, "head"))
            {
                routeInfo = await routeMatch("HEAD").ConfigureAwait(false);

                if (routeInfo == null)
                {
                    routeInfo = await routeMatch("GET").ConfigureAwait(false);
                }
            }
            else if (context.RequestInfo.IsCorsPreflightRequest())
            {
                routeInfo = await routeMatch(context.RequestInfo.CrossOriginRequest.AccessControlRequestMethod).ConfigureAwait(false);
            }
            else
            {
                routeInfo = await routeMatch(context.RequestInfo.Method).ConfigureAwait(false);
            }

            if (routeInfo != null)
            {
                context.RequestConfig = MergeConfigurations(defaultRequestConfig, routeInfo.Config);
            }

            return routeInfo;
        }

        /// <summary>Gets the template information.</summary>
        /// <param name="context">The context.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="routes">The routes.</param>
        /// <returns></returns>
        private static async Task<ApiRoutingTemplate> GetTemplateInfo(this ApiRequestContext context, IUriRouteResolver resolver, IApiRoutingTable routes)
        {
            RouteMatch result;
            ApiRoutingTemplate template = null;

            foreach (var route in routes.GetRoutes())
            {
                result = await resolver.ResolveRoute(route.Template, context.RequestInfo.Path).ConfigureAwait(false);

                if (result?.IsMatch ?? false)
                {
                    if (template == null)
                    {
                        template = new ApiRoutingTemplate
                        {
                            EndpointLocations = new List<ApiEndpointLocation>(),
                            Template = route.Template
                        };

                        template.VariablesList.AddRange(route.VariablesList);
                    }

                    template.EndpointLocations.Add(new ApiEndpointLocation
                    {
                        Controller = route.EndpointLocation.Controller,
                        Endpoint = route.EndpointLocation.Endpoint,
                        HttpMethod = route.HttpMethod
                    });
                }
            }

            return template;
        }

        /// <summary>Clones the routing item.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static ApiRoutingItem CloneRoutingItem(ApiRoutingItem item)
        {
            var newitem = new ApiRoutingItem
            {
                EndpointLocation = new ApiEndpointLocation
                {
                    Controller = item.EndpointLocation.Controller,
                    Endpoint = item.EndpointLocation?.Endpoint,
                    HttpMethod = item.EndpointLocation.HttpMethod
                },
                Name = item.Name,
                Template = item.Template,
                Config = item.Config,
                HttpMethod = item.HttpMethod,
            };

            newitem.RouteVariables = new Dictionary<string, string>();
            newitem.VariablesList.AddRange(item.VariablesList);
            return newitem;
        }

        /// <summary>Merges the configurations.</summary>
        /// <param name="defaultConfig">The default configuration.</param>
        /// <param name="endpointConfig">The endpoint configuration.</param>
        /// <returns></returns>
        private static IApiRequestConfiguration MergeConfigurations(IApiRequestConfiguration defaultConfig, IApiRequestConfiguration endpointConfig)
        {
            IApiRequestConfiguration requestConfig = (endpointConfig != null)
                ? endpointConfig.Init()
                : defaultConfig.Init();

            requestConfig.AllowAnonymous = (endpointConfig ?? defaultConfig).AllowAnonymous;
            requestConfig.Deprecated = (endpointConfig ?? defaultConfig).Deprecated;
            requestConfig.FallBackLanguage = (endpointConfig ?? defaultConfig).FallBackLanguage;
            requestConfig.MaxRequestLength = (endpointConfig ?? defaultConfig).MaxRequestLength;
            requestConfig.MaxRequestUriLength = (endpointConfig ?? defaultConfig).MaxRequestUriLength;
            requestConfig.MinRequestLength = (endpointConfig ?? defaultConfig).MinRequestLength;
            requestConfig.ResourceId = (endpointConfig ?? defaultConfig).ResourceId;
            requestConfig.SupportedLanguages = (endpointConfig ?? defaultConfig).SupportedLanguages;

            // Merge CacheDirective
            requestConfig.CacheDirective = defaultConfig.CacheDirective;
            if (endpointConfig.CacheDirective != null)
            {
                if (requestConfig.CacheDirective == null)
                {
                    requestConfig.CacheDirective = endpointConfig.CacheDirective;
                }
                else if (endpointConfig.CacheDirective != null)
                {
                    requestConfig.CacheDirective.Cacheability = endpointConfig.CacheDirective.Cacheability ?? requestConfig.CacheDirective.Cacheability;
                    requestConfig.CacheDirective.CacheLocation = endpointConfig.CacheDirective.CacheLocation ?? requestConfig.CacheDirective.CacheLocation;
                    requestConfig.CacheDirective.ExpirationSeconds = endpointConfig.CacheDirective.ExpirationSeconds ?? requestConfig.CacheDirective.ExpirationSeconds;
                }
            }

            // Merge CrossOriginConfig
            requestConfig.CrossOriginConfig = defaultConfig.CrossOriginConfig;
            if (endpointConfig.CrossOriginConfig != null)
            {
                if (requestConfig.CrossOriginConfig == null)
                {
                    requestConfig.CrossOriginConfig = endpointConfig.CrossOriginConfig;
                }
                else if (endpointConfig.CrossOriginConfig != null)
                {
                    requestConfig.CrossOriginConfig.AllowCredentials = endpointConfig.CrossOriginConfig.AllowCredentials ?? requestConfig.CrossOriginConfig.AllowCredentials;
                    requestConfig.CrossOriginConfig.AllowedOrigins = endpointConfig.CrossOriginConfig.AllowedOrigins ?? requestConfig.CrossOriginConfig.AllowedOrigins;
                    requestConfig.CrossOriginConfig.ExposeHeaders = endpointConfig.CrossOriginConfig.ExposeHeaders ?? requestConfig.CrossOriginConfig.ExposeHeaders;
                }
            }

            // Merge HeaderValidationConfig
            requestConfig.HeaderValidationConfig = defaultConfig.HeaderValidationConfig;
            if (endpointConfig.HeaderValidationConfig != null)
            {
                if (requestConfig.HeaderValidationConfig == null)
                {
                    requestConfig.HeaderValidationConfig = endpointConfig.HeaderValidationConfig;
                }
                else if (endpointConfig.HeaderValidationConfig != null)
                {
                    requestConfig.HeaderValidationConfig.MaxHeaderLength = endpointConfig.HeaderValidationConfig.MaxHeaderLength ?? requestConfig.HeaderValidationConfig.MaxHeaderLength;
                }
            }

            // Merge HttpConfig
            requestConfig.HttpConfig = defaultConfig.HttpConfig;
            if (endpointConfig.HttpConfig != null)
            {
                if (requestConfig.HttpConfig == null)
                {
                    requestConfig.HttpConfig = endpointConfig.HttpConfig;
                }
                else if (endpointConfig.HttpConfig != null)
                {
                    requestConfig.HttpConfig.RequireSSL = endpointConfig.HttpConfig.RequireSSL ?? requestConfig.HttpConfig.RequireSSL;
                    requestConfig.HttpConfig.SupportedVersions = endpointConfig.HttpConfig.SupportedVersions ?? requestConfig.HttpConfig.SupportedVersions;
                }
            }

            return requestConfig;
        }
    }
}
