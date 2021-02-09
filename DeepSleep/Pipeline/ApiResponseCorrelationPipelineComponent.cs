namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCorrelationPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorrelationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCorrelationPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            finally
            {
                var context = contextResolver
                     .GetContext()
                     .SetThreadCulure();

                var defaultRequestConfig = context?.RequestServices?.GetService<IDeepSleepRequestConfiguration>();

                await context.ProcessHttpResponseCorrelation(defaultRequestConfig).ConfigureAwait(false);
            }
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
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseCorrelation(this ApiRequestContext context, IDeepSleepRequestConfiguration defaultRequestConfiguration)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Request?.CorrelationId != null)
                {
                    var useCorrelationIdHeader = context.Configuration?.UseCorrelationIdHeader
                        ?? defaultRequestConfiguration?.UseCorrelationIdHeader
                        ?? ApiRequestContext.GetDefaultRequestConfiguration().UseCorrelationIdHeader
                        ?? true;

                    if (useCorrelationIdHeader)
                    {
                        context.Response.AddHeader("X-CorrelationId", context.Request.CorrelationId);
                    }
                }
            }

            return Task.FromResult(true);
        }
    }
}
