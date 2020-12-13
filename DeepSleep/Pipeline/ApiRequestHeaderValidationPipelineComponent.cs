namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using System.Globalization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestHeaderValidationPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestHeaderValidationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestHeaderValidationPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestHeaderValidationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request header validation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestHeaderValidation(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestHeaderValidationPipelineComponent>();
        }

        /// <summary>Processes the HTTP request header validation.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpRequestHeaderValidation(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo.Headers != null)
                {
                    var maxHeaderLength = context.RequestConfig?.HeaderValidationConfig?.MaxHeaderLength ?? 0;

                    if (maxHeaderLength > 0)
                    {
                        foreach (var header in context.RequestInfo.Headers)
                        {
                            if (header.Value?.Length > maxHeaderLength)
                            {
                                context.ResponseInfo.StatusCode = 431;
                                return Task.FromResult(false);
                            }
                        }
                    }
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
