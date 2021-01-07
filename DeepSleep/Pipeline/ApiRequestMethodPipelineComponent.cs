namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestMethodPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestMethodPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestMethodPipelineComponent(ApiRequestDelegate next)
              : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var routes = context?.RequestServices.GetService<IApiRoutingTable>();
            var resolver = context?.RequestServices.GetService<IUriRouteResolver>();
            var defaultRequestConfig = context?.RequestServices?.GetService<IApiRequestConfiguration>();

            if (await context.ProcessHttpRequestMethod(routes, resolver, defaultRequestConfig).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestMethodPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request method.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestMethod(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestMethodPipelineComponent>();
        }

        /// <summary>Processes the HTTP request method.</summary>
        /// <param name="context">The context.</param>
        /// <param name="routes">The routes.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestMethod(this ApiRequestContext context, 
            IApiRoutingTable routes, 
            IUriRouteResolver resolver, 
            IApiRequestConfiguration defaultRequestConfiguration)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                // Templates exist for thies route
                if ((context.Routing?.Template?.Locations?.Count ?? 0) > 0)
                {
                    // A route was not found for the template 
                    if (context.Routing.Route == null)
                    {
                        var methods = context.Routing.Template.Locations
                            .Where(e => !string.IsNullOrWhiteSpace(e?.HttpMethod))
                            .Select(e => e.HttpMethod.ToUpper())
                            .Distinct()
                            .ToList();

                        if (methods.Contains("GET") && !methods.Contains("HEAD"))
                        {
                            if (resolver != null)
                            {
                                var match = await resolver.MatchRoute(
                                    routes,
                                    "GET",
                                    context.Request.Path).ConfigureAwait(false);

                                if (match != null)
                                {
                                    var enableHeadForGetRequests = match.Configuration?.EnableHeadForGetRequests
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

                        context.Runtime.Internals.IsMethodNotFound = true;
                        context.Response.AddHeader("Allow", string.Join(", ", methods));
                        context.Response.StatusCode = 405;

                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
