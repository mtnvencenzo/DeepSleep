namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using System.Globalization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestUriValidationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestUriValidationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestUriValidationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestUriValidation().ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestUriValidationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request URI validation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestUriValidation(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestUriValidationPipelineComponent>();
        }

        /// <summary>Processes the HTTP request URI validation.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpRequestUriValidation(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                int max = 2083;

                if (!string.IsNullOrWhiteSpace(context.RequestInfo?.RequestUri))
                {
                    if (context.RequestInfo.RequestUri.Length > max)
                    {
                        context.ErrorMessages.Add(string.Format(ValidationErrors.RequestUriLengthExceeded, max.ToString(CultureInfo.InvariantCulture)));
                        context.ResponseInfo.StatusCode = 414;

                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
