namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using DeepSleep.Resources;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestBodyBindingPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestBodyBindingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestBodyBindingPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormatStreamReaderWriterFactory formatterFactory, IApiResponseMessageConverter responseMessageConverter)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestBodyBinding(formatterFactory, responseMessageConverter).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestBodyBindingPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request body binding.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestBodyBinding(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestBodyBindingPipelineComponent>();
        }

        /// <summary>Processes the HTTP request body binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestBodyBinding(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo.Method?.In(StringComparison.InvariantCultureIgnoreCase, "post", "patch", "put") ?? false)
                {
                    if (!context.RequestInfo.ContentLength.HasValue)
                    {
                        //logger?.LogWarning($"Request did not contain a Content-Length header for request method {context.RequestInfo.Method}, issueing HTTP 411 Length Required");

                        context.ResponseInfo.StatusCode = 411;

                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        //logger?.LogWarning($"Request did not contain a Content-Type header for request with a content length provided, issueing HTTP 422 Unprocessable Entity");

                        context.ResponseInfo.StatusCode = 422;

                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && context.RequestInfo.InvocationContext?.BodyModelType == null)
                    {
                        //logger?.LogWarning($"Bpdy model type not available but a body was supplied in the request, issueing HTTP 413 Payload Too Large");

                        context.ResponseInfo.StatusCode = 413;

                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && !string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        IFormatStreamReaderWriter formatter = (formatterFactory != null)
                            ? await formatterFactory.GetMediaTypeFormatter(context.RequestInfo.ContentType, out var _).ConfigureAwait(false)
                            : null;

                        if (formatter == null)
                        {
                            //logger?.LogWarning($"Could not find a formatter for the request Content-Type, issueing HTTP 415 Unsupported Media Type");

                            context.ResponseInfo.StatusCode = 415;

                            return false;
                        }

                        try
                        {
                            context.RequestInfo.InvocationContext.BodyModel = await formatter.ReadType(context.RequestInfo.Body, context.RequestInfo.InvocationContext.BodyModelType)
                                .ConfigureAwait(false);

                            //logger?.LogInformation($"Body model type: {context.RequestInfo.InvocationContext.BodyModelType.FullName} has successfully been bound");
                        }
                        catch (System.Exception ex)
                        {
                            if (ex == null)
                            {
                            }
                            //logger?.LogWarning($"Could not deserialize the request body using Content-Type: {context.RequestInfo.ContentType} and formatter {formatter.GetType().Name}, issueing HTTP 400 Bad Request");

                            context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(ValidationErrors.RequestBodyDeserializationError));

                            context.ResponseInfo.StatusCode = 400;

                            return false;
                        }
                    }
                }

                return true;
            }

            return false;
        }
    }
}
