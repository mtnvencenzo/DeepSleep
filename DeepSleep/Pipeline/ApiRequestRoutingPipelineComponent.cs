namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using Microsoft.Extensions.Logging;
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
        /// <param name="logger">The logger.</param>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiRoutingTable routes, IUriRouteResolver resolver, IApiRequestConfiguration defaultRequestConfig, ILogger<ApiRequestRoutingPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestRouting(routes, resolver, defaultRequestConfig, logger).ConfigureAwait(false))
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
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpRequestRouting(this ApiRequestContext context, IApiRoutingTable routes, IUriRouteResolver resolver, IApiRequestConfiguration defaultRequestConfig, ILogger logger)
        {
            logger?.LogInformation("LogDebug");

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

            requestConfig.AllowAnonymous = endpointConfig?.AllowAnonymous ?? defaultConfig.AllowAnonymous;
            requestConfig.Deprecated = endpointConfig?.Deprecated ?? defaultConfig.Deprecated;
            requestConfig.FallBackLanguage = endpointConfig?.FallBackLanguage ?? defaultConfig.FallBackLanguage;
            requestConfig.MaxRequestLength = endpointConfig?.MaxRequestLength ?? defaultConfig.MaxRequestLength;
            requestConfig.MaxRequestUriLength = endpointConfig?.MaxRequestUriLength ?? defaultConfig.MaxRequestUriLength;
            requestConfig.MinRequestLength = endpointConfig?.MinRequestLength ?? defaultConfig.MinRequestLength;
            requestConfig.ResourceId = endpointConfig?.ResourceId ?? defaultConfig.ResourceId;
            requestConfig.SupportedLanguages = endpointConfig?.SupportedLanguages ?? defaultConfig.SupportedLanguages;
            requestConfig.SupportedAuthenticationSchemes = endpointConfig?.SupportedAuthenticationSchemes ?? defaultConfig.SupportedAuthenticationSchemes;

            // Merge CacheDirective
            if (defaultConfig?.CacheDirective != null || endpointConfig?.CacheDirective != null)
            {
                requestConfig.CacheDirective = new HttpCacheDirective
                {
                    Cacheability = endpointConfig?.CacheDirective?.Cacheability ?? defaultConfig?.CacheDirective?.Cacheability,
                    CacheLocation = endpointConfig?.CacheDirective?.CacheLocation ?? defaultConfig?.CacheDirective?.CacheLocation,
                    ExpirationSeconds = endpointConfig?.CacheDirective?.ExpirationSeconds ?? defaultConfig?.CacheDirective?.ExpirationSeconds
                };
            }

            // Merge CrossOriginConfig
            if (defaultConfig?.CrossOriginConfig != null || endpointConfig?.CrossOriginConfig != null)
            {
                requestConfig.CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = endpointConfig?.CrossOriginConfig?.AllowCredentials ?? defaultConfig?.CrossOriginConfig?.AllowCredentials,
                    AllowedOrigins = endpointConfig?.CrossOriginConfig?.AllowedOrigins ?? defaultConfig?.CrossOriginConfig?.AllowedOrigins,
                    ExposeHeaders = endpointConfig?.CrossOriginConfig?.ExposeHeaders ?? defaultConfig?.CrossOriginConfig?.ExposeHeaders
                };
            }

            // Merge HeaderValidationConfig
            if (defaultConfig?.HeaderValidationConfig != null || endpointConfig?.HeaderValidationConfig != null)
            {
                requestConfig.HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = endpointConfig?.HeaderValidationConfig?.MaxHeaderLength ?? defaultConfig?.HeaderValidationConfig?.MaxHeaderLength
                };
            }

            // Merge HttpConfig
            if (defaultConfig?.HttpConfig != null || endpointConfig?.HttpConfig != null)
            {
                requestConfig.HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = endpointConfig?.HttpConfig?.RequireSSL ?? defaultConfig?.HttpConfig?.RequireSSL,
                    SupportedVersions = endpointConfig?.HttpConfig?.SupportedVersions ?? defaultConfig?.HttpConfig?.SupportedVersions
                };
            }

            // Merge Resource Authorization Config
            if (defaultConfig?.ResourceAuthorizationConfig != null || endpointConfig?.ResourceAuthorizationConfig != null)
            {
                requestConfig.ResourceAuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = endpointConfig?.ResourceAuthorizationConfig?.Policy ?? defaultConfig?.ResourceAuthorizationConfig?.Policy
                };

                if (endpointConfig?.ResourceAuthorizationConfig != null)
                {
                    requestConfig.ResourceAuthorizationConfig.Policy = endpointConfig.ResourceAuthorizationConfig.Policy;
                }
                else if (defaultConfig?.ResourceAuthorizationConfig != null)
                {
                    requestConfig.ResourceAuthorizationConfig.Policy = defaultConfig.ResourceAuthorizationConfig.Policy;
                }


                if (endpointConfig?.ResourceAuthorizationConfig != null)
                {
                    if (endpointConfig.ResourceAuthorizationConfig.Roles != null)
                    {
                        requestConfig.ResourceAuthorizationConfig.Roles = new List<string>();
                        foreach (var role in endpointConfig.ResourceAuthorizationConfig.Roles)
                        {
                            requestConfig.ResourceAuthorizationConfig.Roles.Add(role);
                        }
                    }
                }
                else if (defaultConfig?.ResourceAuthorizationConfig != null)
                {
                    if (defaultConfig.ResourceAuthorizationConfig.Roles != null)
                    {
                        requestConfig.ResourceAuthorizationConfig.Roles = new List<string>();
                        foreach (var role in defaultConfig.ResourceAuthorizationConfig.Roles)
                        {
                            requestConfig.ResourceAuthorizationConfig.Roles.Add(role);
                        }
                    }
                }
            }

            return requestConfig;
        }
    }
}
