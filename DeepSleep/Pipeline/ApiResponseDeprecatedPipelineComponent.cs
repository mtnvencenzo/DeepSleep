namespace DeepSleep.Pipeline
{
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
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();
            
            await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
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
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseDeprecated(this ApiRequestContext context)
        {
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
