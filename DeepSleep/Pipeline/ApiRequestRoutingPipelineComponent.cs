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
            var context = contextResolver.GetContext();

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
        private static async Task<ApiRoutingItem> GetRouteInfo(this ApiRequestContext context, 
            IUriRouteResolver resolver, 
            IApiRoutingTable routes, 
            IApiRequestConfiguration defaultRequestConfig)
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
                context.RequestConfig = MergeConfigurations(context, defaultRequestConfig, routeInfo.Config);
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
                Template = item.Template,
                Config = item.Config,
                HttpMethod = item.HttpMethod,
            };

            newitem.RouteVariables = new Dictionary<string, string>();
            item.VariablesList.ForEach(v => newitem.VariablesList.Add(v));
            return newitem;
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

            requestConfig.AllowRequestBodyWhenNoModelDefined = endpointConfig?.AllowRequestBodyWhenNoModelDefined
                ?? defaultConfig?.AllowRequestBodyWhenNoModelDefined
                ?? systemConfig.AllowRequestBodyWhenNoModelDefined;

            requestConfig.RequireContentLengthOnRequestBodyRequests = endpointConfig?.RequireContentLengthOnRequestBodyRequests
                ?? defaultConfig?.RequireContentLengthOnRequestBodyRequests
                ?? systemConfig.RequireContentLengthOnRequestBodyRequests;

            requestConfig.FallBackLanguage = endpointConfig?.FallBackLanguage
                ?? defaultConfig?.FallBackLanguage
                ?? systemConfig.FallBackLanguage;

            requestConfig.MaxRequestLength = endpointConfig?.MaxRequestLength
                ?? defaultConfig?.MaxRequestLength
                ?? systemConfig.MaxRequestLength;

            requestConfig.MaxRequestUriLength = endpointConfig?.MaxRequestUriLength
                ?? defaultConfig?.MaxRequestUriLength
                ?? systemConfig.MaxRequestUriLength;

            requestConfig.MinRequestLength = endpointConfig?.MinRequestLength
                ?? defaultConfig?.MinRequestLength
                ?? systemConfig.MinRequestLength;

            requestConfig.SupportedLanguages = new List<string>(endpointConfig?.SupportedLanguages ?? defaultConfig?.SupportedLanguages ?? systemConfig.SupportedLanguages);

            requestConfig.SupportedAuthenticationSchemes = new List<string>(endpointConfig?.SupportedAuthenticationSchemes ?? defaultConfig?.SupportedAuthenticationSchemes ?? systemConfig.SupportedAuthenticationSchemes);

            // ----------------------------
            // Merge CacheDirective
            // ----------------------------
            if (endpointConfig?.CacheDirective != null || defaultConfig?.CacheDirective != null)
            {
                requestConfig.CacheDirective = new HttpCacheDirective
                {
                    Cacheability = endpointConfig?.CacheDirective?.Cacheability
                        ?? defaultConfig?.CacheDirective?.Cacheability
                        ?? systemConfig.CacheDirective?.Cacheability,
                    CacheLocation = endpointConfig?.CacheDirective?.CacheLocation
                        ?? defaultConfig?.CacheDirective?.CacheLocation
                        ?? systemConfig.CacheDirective?.CacheLocation,
                    ExpirationSeconds = endpointConfig?.CacheDirective?.ExpirationSeconds
                        ?? defaultConfig?.CacheDirective?.ExpirationSeconds
                        ?? systemConfig.CacheDirective?.ExpirationSeconds
                };
            }
            else
            {
                requestConfig.CacheDirective = systemConfig.CacheDirective;
            }


            // ----------------------------
            // Merge CrossOriginConfig
            // ----------------------------
            if (endpointConfig?.CrossOriginConfig != null || defaultConfig?.CrossOriginConfig != null)
            {
                requestConfig.CrossOriginConfig = new CrossOriginConfiguration
                {
                    AllowCredentials = endpointConfig?.CrossOriginConfig?.AllowCredentials 
                        ?? defaultConfig?.CrossOriginConfig?.AllowCredentials
                        ?? systemConfig?.CrossOriginConfig?.AllowCredentials,
                    AllowedOrigins = new List<string>(endpointConfig?.CrossOriginConfig?.AllowedOrigins ?? defaultConfig?.CrossOriginConfig?.AllowedOrigins ?? systemConfig.CrossOriginConfig.AllowedOrigins),
                    ExposeHeaders = new List<string>(endpointConfig?.CrossOriginConfig?.ExposeHeaders ?? defaultConfig?.CrossOriginConfig?.ExposeHeaders ?? systemConfig.CrossOriginConfig.ExposeHeaders),
                    AllowedHeaders = new List<string>(endpointConfig?.CrossOriginConfig?.AllowedHeaders ?? defaultConfig?.CrossOriginConfig?.AllowedHeaders ?? systemConfig.CrossOriginConfig.AllowedHeaders)
                };
            }
            else
            {
                requestConfig.CrossOriginConfig = systemConfig.CrossOriginConfig;
            }


            // ----------------------------
            // Merge HeaderValidationConfig
            // ----------------------------
            if (defaultConfig?.HeaderValidationConfig != null || endpointConfig?.HeaderValidationConfig != null)
            {
                requestConfig.HeaderValidationConfig = new ApiHeaderValidationConfiguration
                {
                    MaxHeaderLength = endpointConfig?.HeaderValidationConfig?.MaxHeaderLength 
                        ?? defaultConfig?.HeaderValidationConfig?.MaxHeaderLength
                        ?? systemConfig.HeaderValidationConfig.MaxHeaderLength
                };
            }
            else
            {
                requestConfig.HeaderValidationConfig = systemConfig.HeaderValidationConfig;
            }


            // ----------------------------
            // Merge HttpConfig
            // ----------------------------
            if (endpointConfig?.HttpConfig != null || defaultConfig?.HttpConfig != null)
            {
                requestConfig.HttpConfig = new ApiHttpConfiguration
                {
                    RequireSSL = endpointConfig?.HttpConfig?.RequireSSL 
                        ?? defaultConfig?.HttpConfig?.RequireSSL
                        ?? systemConfig.HttpConfig.RequireSSL,
                    SupportedVersions = endpointConfig?.HttpConfig?.SupportedVersions 
                        ?? defaultConfig?.HttpConfig?.SupportedVersions
                        ?? systemConfig.HttpConfig.SupportedVersions
                };
            }
            else
            {
                requestConfig.HttpConfig = systemConfig.HttpConfig;
            }



            // -----------------------------------
            // Merge Resource Authorization Config
            // -----------------------------------
            if (endpointConfig?.AuthorizationConfig != null || defaultConfig?.AuthorizationConfig != null)
            {
                requestConfig.AuthorizationConfig = new ResourceAuthorizationConfiguration
                {
                    Policy = endpointConfig?.AuthorizationConfig?.Policy 
                        ?? defaultConfig?.AuthorizationConfig?.Policy
                        ?? systemConfig.AuthorizationConfig.Policy
                };
            }
            else
            {
                requestConfig.AuthorizationConfig = systemConfig.AuthorizationConfig;
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
