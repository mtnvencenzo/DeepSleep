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
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormatStreamReaderWriterFactory formatterFactory)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestBodyBinding(formatterFactory).ConfigureAwait(false))
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
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestBodyBinding(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo.Method?.In(StringComparison.InvariantCultureIgnoreCase, "post", "patch", "put") ?? false)
                {
                    if (!context.RequestInfo.ContentLength.HasValue)
                    {
                        context.ResponseInfo.StatusCode = 411;

                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        context.ResponseInfo.StatusCode = 422;

                        return false;
                    }

                    if (context.RequestInfo.ContentLength > 0 && context.RequestInfo.InvocationContext?.BodyModelType == null)
                    {
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
                            context.ResponseInfo.StatusCode = 415;

                            return false;
                        }

                        try
                        {
                            context.RequestInfo.InvocationContext.BodyModel = await formatter.ReadType(context.RequestInfo.Body, context.RequestInfo.InvocationContext.BodyModelType)
                                .ConfigureAwait(false);
                        }
                        catch
                        {
                            context.ErrorMessages.Add(ValidationErrors.RequestBodyDeserializationError);

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
