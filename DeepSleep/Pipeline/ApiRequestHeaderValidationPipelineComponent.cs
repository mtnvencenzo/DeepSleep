namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestHeaderValidationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestHeaderValidationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestHeaderValidationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiResponseMessageConverter responseMessageConverter, ILogger<ApiRequestHeaderValidationPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestHeaderValidation(responseMessageConverter, logger).ConfigureAwait(false))
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
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpRequestHeaderValidation(this ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter, ILogger logger)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedHeaderError = false;

                if (context.RequestInfo.Headers != null)
                {
                    var maxHeaderLength = context.RequestConfig?.HeaderValidationConfig?.MaxHeaderLength ?? 0;

                    if (maxHeaderLength > 0)
                    {
                        foreach (var header in context.RequestInfo.Headers)
                        {
                            if (header.Value.Length > maxHeaderLength)
                            {
                                addedHeaderError = true;
                                context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.HeaderLengthExceeded,
                                    header.Name,
                                    maxHeaderLength.ToString())));
                            }
                        }
                    }
                }

                if (addedHeaderError)
                {
                    logger?.LogWarning($"Header validation failed, issueing HTTP 431 Request Header Fields Too Large");

                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 431
                    };
                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
