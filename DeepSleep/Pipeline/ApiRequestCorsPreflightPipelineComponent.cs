namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestCorsPreflightPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestCorsPreflightPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestCorsPreflightPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            var routes = context.RequestServices.GetService<IApiRoutingTable>();
            var resolver = context.RequestServices.GetService<IUriRouteResolver>();
            var defaultRequestConfiguration = context.RequestServices.GetService<IApiRequestConfiguration>();

            if (await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(routes, resolver, defaultRequestConfiguration).ConfigureAwait(false))
            {
                await this.apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestCorsPreflightPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request cors preflight.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestCorsPreflight(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestCorsPreflightPipelineComponent>();
        }

        /// <summary>Processes the HTTP request cross origin resource sharing preflight.</summary>
        /// <param name="context">The context.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestCrossOriginResourceSharingPreflight(
            this ApiRequestContext context, 
            IApiRoutingTable routes, 
            IUriRouteResolver resolver,
            IApiRequestConfiguration defaultRequestConfiguration)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Request?.IsCorsPreflightRequest() ?? false)
                {
                    var methods = (context.Routing?.Template?.Locations ?? new List<ApiEndpointLocation>())
                        .Where(r => !string.IsNullOrWhiteSpace(r.HttpMethod))
                        .Select(r => r.HttpMethod.ToUpper())
                        .Distinct()
                        .ToList();

                    // Need to include the auto-enabled HEAD method if configured on the get endpoint (if available)
                    var hasGet = methods.FirstOrDefault(m => m.Equals("GET", StringComparison.InvariantCultureIgnoreCase)) != null;
                    if (hasGet)
                    {
                        var hasHead = methods.FirstOrDefault(m => m.Equals("HEAD", StringComparison.InvariantCultureIgnoreCase)) != null;
                        if (!hasHead)
                        {
                            if (resolver != null)
                            {
                                var getMatch = await resolver.MatchRoute(
                                    routes,
                                    "GET",
                                    context.Request.Path).ConfigureAwait(false);

                                if (getMatch != null)
                                {
                                    var enableHeadForGetRequests = getMatch.Configuration?.EnableHeadForGetRequests
                                        ?? defaultRequestConfiguration?.EnableHeadForGetRequests
                                        ?? ApiRequestContext.GetDefaultRequestConfiguration().EnableHeadForGetRequests
                                        ?? true;

                                    if (enableHeadForGetRequests)
                                    {
                                        methods.Add("HEAD");
                                    }
                                }
                            }
                        }
                    }


                    context.Response.StatusCode = 200;

                    context.Response.AddHeader("Access-Control-Allow-Methods", string.Join(", ", methods).Trim());

                    if (!string.IsNullOrWhiteSpace(context.Request?.CrossOriginRequest?.AccessControlRequestHeaders))
                    {
                        var allowHeaders = (context.Configuration?.CrossOriginConfig?.AllowedHeaders ?? new string[] { })
                            .Distinct()
                            .Where(i => !string.IsNullOrWhiteSpace(i))
                            .Select(i => i.Trim())
                            .ToList();

                        if (allowHeaders.Count > 0 && allowHeaders.Contains("*"))
                        {
                            context.Response.AddHeader("Access-Control-Allow-Headers", context.Request.CrossOriginRequest.AccessControlRequestHeaders);
                        }
                        else
                        {
                            context.Response.AddHeader("Access-Control-Allow-Headers", string.Join(", ", allowHeaders));
                        }
                    }

                    if (context.Configuration?.CrossOriginConfig?.MaxAgeSeconds.HasValue ?? false)
                    {
                        context.Response.AddHeader("Access-Control-Max-Age", $"{context.Configuration.CrossOriginConfig.MaxAgeSeconds.Value}");
                    }


                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
