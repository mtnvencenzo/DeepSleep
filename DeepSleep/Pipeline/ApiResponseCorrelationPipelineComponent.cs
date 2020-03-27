namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCorrelationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorrelationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCorrelationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiResponseCorrelationPipelineComponent> logger)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            await context.ProcessHttpResponseCorrelation(logger).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseCorrelationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response correlation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseCorrelation(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseCorrelationPipelineComponent>();
        }

        /// <summary>Processes the HTTP response correlation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseCorrelation(this ApiRequestContext context, ILogger logger)
        {
            logger?.LogInformation("Invoked");

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo?.CorrelationId != null)
                {
                    context.ResponseInfo.AddHeader("X-CorrelationId", context.RequestInfo.CorrelationId);
                }
            }

            return Task.FromResult(true);
        }
    }
}
