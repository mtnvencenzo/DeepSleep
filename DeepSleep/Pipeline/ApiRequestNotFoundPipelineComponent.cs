namespace DeepSleep.Pipeline
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestNotFoundPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestNotFoundPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestNotFoundPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestNotFound().ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            } 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestNotFoundPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request not found.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestNotFound(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestNotFoundPipelineComponent>();
        }

        /// <summary>Processes the HTTP request not found.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestNotFound(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if ((context.RouteInfo?.TemplateInfo?.EndpointLocations?.Count ?? 0) == 0)
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 404
                    };

                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
