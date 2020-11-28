namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestCanceledPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestCanceledPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestCanceledPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiRequestCanceledPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestCanceled(logger).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestCanceledPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request canceled.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestCanceled(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestCanceledPipelineComponent>();
        }

        /// <summary>Processes the HTTP request canceled.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestCanceled(this ApiRequestContext context, ILogger logger)
        {
            if (context.RequestAborted.IsCancellationRequested)
            {
                logger?.LogInformation($"Request has been cancelled by client, issuing HTTP 408 Request Timeout");

                context.ResponseInfo.StatusCode = 408;

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
