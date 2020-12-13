namespace DeepSleep.Pipeline
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseMessagePipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseMessagePipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseMessagePipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            await context.ProcessHttpResponseMessages().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseMessagePipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response messages.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseMessages(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseMessagePipelineComponent>();
        }

        /// <summary>Processes the HTTP response messages.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpResponseMessages(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if ((context.ErrorMessages?.Count ?? 0) > 0)
                {
                    context.ErrorMessages = context.ErrorMessages
                        .Distinct()
                        .ToList();

                    if (context.RequestConfig?.ApiErrorResponseProvider != null)
                    {
                        var provider = context.RequestConfig.ApiErrorResponseProvider(context.RequestServices);

                        if (provider != null)
                        {
                            await provider.Process(context).ConfigureAwait(false);
                        }
                    }
                }
            }

            return true;
        }
    }
}
