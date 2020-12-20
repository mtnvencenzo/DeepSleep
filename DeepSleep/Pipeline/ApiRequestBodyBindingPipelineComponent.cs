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
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestBodyBindingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestBodyBindingPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            var formatterFactory = context?.RequestServices?.GetService<IFormatStreamReaderWriterFactory>();

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
                        if (context.RequestConfig?.RequireContentLengthOnRequestBodyRequests ?? true)
                        {
                            context.ResponseInfo.StatusCode = 411;
                            return false;
                        }
                    }

                    if (context.RequestInfo.ContentLength > 0 && string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        context.ResponseInfo.StatusCode = 450;
                        return false;
                    }

                    if (context.RequestConfig?.MaxRequestLength > 0 && context.RequestInfo.ContentLength > 0)
                    {
                        if (context.RequestInfo.ContentLength > context.RequestConfig.MaxRequestLength)
                        {
                            context.ResponseInfo.StatusCode = 413;
                            return false;
                        }
                    }

                    if (context.RequestInfo.ContentLength > 0 && context.RequestInfo.InvocationContext?.BodyModelType == null)
                    {
                        if (!(context.RequestConfig?.AllowRequestBodyWhenNoModelDefined ?? false))
                        {
                            context.ResponseInfo.StatusCode = 413;
                            return false;
                        }
                    }

                    if (context.RequestInfo.InvocationContext?.BodyModelType != null && context.RequestInfo.ContentLength > 0 && !string.IsNullOrWhiteSpace(context.RequestInfo.ContentType))
                    {
                        IFormatStreamReaderWriter formatter = null;

                        if (formatterFactory != null)
                        {
                            formatter = await formatterFactory.GetContentTypeFormatter(
                                contentTypeHeader: context.RequestInfo.ContentType,
                                formatterType: out var _,
                                readableMediaTypes: context.RequestConfig?.ReadWriteConfiguration?.ReadableMediaTypes).ConfigureAwait(false);
                        }

                        if (context.RequestConfig.ReadWriteConfiguration?.ReaderResolver != null)
                        {
                            var overrides = await context.RequestConfig.ReadWriteConfiguration.ReaderResolver(new ResolvedFormatterArguments(context, formatter)).ConfigureAwait(false);

                            if (overrides?.Formatters != null)
                            {
                                formatter = await formatterFactory.GetContentTypeFormatter(
                                    contentTypeHeader: context.RequestInfo.ContentType,
                                    formatterType: out var _,
                                    readableFormatters: overrides.Formatters,
                                    readableMediaTypes: context.RequestConfig?.ReadWriteConfiguration?.ReadableMediaTypes).ConfigureAwait(false);
                            }
                        }

                        if (formatter == null)
                        {
                            context.ResponseInfo.StatusCode = 415;
                            return false;
                        }

                        try
                        {
                            context.RequestInfo.InvocationContext.BodyModel = await formatter.ReadType(
                                stream: context.RequestInfo.Body, 
                                objType: context.RequestInfo.InvocationContext.BodyModelType,
                                options: null).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            context.AddException(ex);

                            if (ex.GetType().Name.Contains("BadHttpRequestException"))
                            {
                                context.ResponseInfo.StatusCode = 413;
                            }
                            else
                            {
                                context.ResponseInfo.StatusCode = 450;
                            }

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
