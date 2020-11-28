namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestMethodPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestMethodPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestMethodPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiRequestMethodPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestMethod(logger).ConfigureAwait(false))
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
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestMethod(this ApiRequestContext context, ILogger logger)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                // Templates exist for thies route
                if ((context.RouteInfo?.TemplateInfo?.EndpointLocations?.Count ?? 0) > 0)
                {
                    // A route was not found for the template 
                    if (context.RouteInfo.RoutingItem == null)
                    {
                        var methods = context.RouteInfo.TemplateInfo.EndpointLocations
                            .Where(e => !string.IsNullOrWhiteSpace(e?.HttpMethod))
                            .Select(e => e.HttpMethod.ToUpper())
                            .Distinct()
                            .ToList();

                        if (methods.Contains("GET") && !methods.Contains("HEAD"))
                        {
                            methods.Add("HEAD");
                        }

                        context.ResponseInfo.AddHeader("Allow", string.Join(", ", methods));
                        
                        logger?.LogWarning($"Request method {context.RequestInfo.Method} could be not matched with template {context.RouteInfo.TemplateInfo}.  Available methods are {string.Join(", ", methods)}, issueing HTTP 405 Method Not Allowed");

                        context.ResponseInfo.StatusCode = 405;

                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
