namespace DeepSleep.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestCorsPreflightPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestCorsPreflightPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestCorsPreflightPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestCrossOriginResourceSharingPreflight().ConfigureAwait(false))
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

        /// <summary>
        /// Processes the HTTP request cross origin resource sharing preflight.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestCrossOriginResourceSharingPreflight(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo?.IsCorsPreflightRequest() ?? false)
                {
                    var methods = (context.RouteInfo?.TemplateInfo?.EndpointLocations ?? new List<ApiEndpointLocation>())
                        .Where(r => !string.IsNullOrWhiteSpace(r.HttpMethod))
                        .Select(r => r.HttpMethod.ToUpper())
                        .Distinct()
                        .ToArray();

                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 200
                    };

                    context.ResponseInfo.AddHeader("Access-Control-Allow-Methods", string.Join(", ", methods).Trim());

                    if (!string.IsNullOrWhiteSpace(context.RequestInfo?.CrossOriginRequest?.AccessControlRequestHeaders))
                    {
                        context.ResponseInfo.AddHeader("Access-Control-Allow-Headers", context.RequestInfo.CrossOriginRequest.AccessControlRequestHeaders);
                    }

                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
