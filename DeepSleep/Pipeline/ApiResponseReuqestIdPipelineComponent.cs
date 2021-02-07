namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseReuqestIdPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorrelationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseReuqestIdPipelineComponent(ApiRequestDelegate next)
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

                await context.ProcessHttpResponseRequestId(defaultRequestConfig).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseReuqestIdPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response correlation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseRequesId(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseReuqestIdPipelineComponent>();
        }

        /// <summary>Processes the HTTP response request identifier.</summary>
        /// <param name="context">The context.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseRequestId(this ApiRequestContext context, IDeepSleepRequestConfiguration defaultRequestConfiguration)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!string.IsNullOrWhiteSpace(context.Request.RequestIdentifier))
                {
                    var includeRequestIdHeaderInResponse = context.Configuration?.IncludeRequestIdHeaderInResponse
                        ?? defaultRequestConfiguration?.EnableHeadForGetRequests
                        ?? ApiRequestContext.GetDefaultRequestConfiguration().IncludeRequestIdHeaderInResponse
                        ?? false;

                    if (includeRequestIdHeaderInResponse == true)
                    {
                        context.Response.AddHeader("X-RequestId", context.Request.RequestIdentifier);
                    }
                }
            }

            return Task.FromResult(true);
        }
    }
}
