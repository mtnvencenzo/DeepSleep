namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseDeprecatedPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>Initializes a new instance of the <see cref="ApiResponseDeprecatedPipelineComponent"/> class.</summary>
        /// <param name="next">The next.</param>
        public ApiResponseDeprecatedPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiResponseDeprecatedPipelineComponent> logger)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();
            
            await context.ProcessHttpResponseDeprecated(logger).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseDeprecatedPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response correlation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseDeprecated(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseDeprecatedPipelineComponent>();
        }

        /// <summary>Processes the HTTP response deprecated.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseDeprecated(this ApiRequestContext context, ILogger logger)
        {
            logger?.LogInformation("Invoked");

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestConfig?.Deprecated ?? false)
                {
                    context.ResponseInfo.AddHeader("X-Deprecated", true.ToString().ToLower());
                }
            }

            return Task.FromResult(true);
        }
    }
}
