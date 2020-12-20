namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseBodyWriterPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseBodyWriterPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseBodyWriterPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            var formatterFactory = context?.RequestServices?.GetService<IFormatStreamReaderWriterFactory>();

            await context.ProcessHttpResponseBodyWriting(formatterFactory).ConfigureAwait(false);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseBodyWriterPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response body writer.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseBodyWriter(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseBodyWriterPipelineComponent>();
        }

        /// <summary>Processes the HTTP response body writing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpResponseBodyWriting(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var isWriteableResponse = false;

                if (context.ResponseInfo.ResponseObject != null && formatterFactory != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.RequestInfo.Accept)
                        ? context.RequestInfo.Accept
                        : AcceptHeader.All();


                    var isConditionalRequestMatch = ApiCondtionalMatchType.None;

                    if (string.Equals(context.RequestInfo.Method, "get", System.StringComparison.OrdinalIgnoreCase) && context.RequestInfo.IsHeadRequest() == false)
                    {
                        isConditionalRequestMatch = context.IsConditionalRequestMatch(context.ResponseInfo);
                    }

                    if (isConditionalRequestMatch != ApiCondtionalMatchType.ConditionalGetMatch)
                    {
                        IFormatStreamOptions options = new FormatterOptions
                        {
                            PrettyPrint = context.RequestInfo.PrettyPrint
                        };

                        var formatter = await formatterFactory.GetAcceptableFormatter(
                            acceptHeader: context.RequestConfig?.ReadWriteConfiguration?.AcceptHeaderOverride ?? accept, 
                            writeableMediaTypes: context.RequestConfig?.ReadWriteConfiguration?.WriteableMediaTypes, 
                            formatterType: out var formatterType).ConfigureAwait(false);

                        if (context.RequestConfig.ReadWriteConfiguration?.WriterResolver != null)
                        {
                            var overrides = await context.RequestConfig.ReadWriteConfiguration.WriterResolver(new ResolvedFormatterArguments(context, formatter, options)).ConfigureAwait(false);

                            if (overrides?.Formatters != null)
                            {
                                formatter = await formatterFactory.GetAcceptableFormatter(
                                    acceptHeader: context.RequestConfig?.ReadWriteConfiguration?.AcceptHeaderOverride ?? accept,
                                    writeableMediaTypes: context.RequestConfig?.ReadWriteConfiguration?.WriteableMediaTypes,
                                    writeableFormatters: overrides.Formatters,
                                    formatterType: out formatterType).ConfigureAwait(false);
                            }
                        }

                        if (formatter != null)
                        {
                            if (formatter.SupportsPrettyPrint && context.RequestInfo.PrettyPrint)
                            {
                                context.ResponseInfo.AddHeader("X-PrettyPrint", (options?.PrettyPrint ?? false).ToString().ToLower());
                            }

                            isWriteableResponse = true;
                            context.ResponseInfo.ResponseWriter = formatter;
                            context.ResponseInfo.ResponseWriterOptions = options;
                            context.ResponseInfo.ContentType = formatterType;
                        }
                    }
                    else
                    {
                        context.ResponseInfo.StatusCode = 304;
                    }
                }

                if (!isWriteableResponse)
                {
                    if (context.ResponseInfo.HasSuccessStatus() && context.ResponseInfo.StatusCode != 202)
                    {
                        context.ResponseInfo.StatusCode = 204;
                    }
                }
            }

            return true;
        }
    }
}
