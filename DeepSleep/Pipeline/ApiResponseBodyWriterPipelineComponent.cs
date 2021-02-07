namespace DeepSleep.Pipeline
{
    using DeepSleep.Media;
    using System.Text;
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

            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var formatterFactory = context?.RequestServices?.GetService<IDeepSleepMediaSerializerFactory>();

            await context.ProcessHttpResponseBodyWriting(contextResolver, formatterFactory).ConfigureAwait(false);
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
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpResponseBodyWriting(this ApiRequestContext context, IApiRequestContextResolver contextResolver, IDeepSleepMediaSerializerFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var isWriteableResponse = false;

                if (context.Response.ResponseObject != null && formatterFactory != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.Request.Accept)
                        ? context.Request.Accept
                        : AcceptHeader.All();

                    var isConditionalRequestMatch = ApiCondtionalMatchType.None;

                    if (string.Equals(context.Request.Method, "get", System.StringComparison.OrdinalIgnoreCase) && context.Request.IsHeadRequest() == false)
                    {
                        isConditionalRequestMatch = context.Request.IsConditionalRequestMatch(context.Response);
                    }

                    if (isConditionalRequestMatch != ApiCondtionalMatchType.ConditionalGetMatch)
                    {
                        Encoding acceptEncoding = null;

                        if (context.Request.AcceptCharset != null as string)
                        {
                            foreach (var value in context.Request.AcceptCharset.Values)
                            {
                                try
                                {
                                    if (!string.IsNullOrWhiteSpace(value.Charset))
                                    {
                                        acceptEncoding = Encoding.GetEncoding(value.Charset);
                                    }

                                    if (acceptEncoding != null)
                                    {
                                        break;
                                    }
                                }
                                catch { }
                            }
                        }

                        IMediaSerializerOptions options = new MediaSerializerOptions
                        {
                            Culture = context.Request.AcceptCulture,
                            Encoding = acceptEncoding ?? Encoding.UTF8
                        };

                        var returnType = context.Response.ResponseObject.GetType();

                        var formatter = await formatterFactory.GetAcceptableFormatter(
                            objType: returnType,
                            acceptHeader: context.Configuration?.ReadWriteConfiguration?.AcceptHeaderOverride ?? accept,
                            writeableMediaTypes: context.Configuration?.ReadWriteConfiguration?.WriteableMediaTypes,
                            formatterType: out var formatterType).ConfigureAwait(false);

                        if (context.Configuration.ReadWriteConfiguration?.WriterResolver != null)
                        {
                            var overrides = await context.Configuration.ReadWriteConfiguration.WriterResolver(context?.RequestServices).ConfigureAwait(false);

                            if (overrides?.Formatters != null)
                            {
                                formatter = await formatterFactory.GetAcceptableFormatter(
                                    objType: returnType,
                                    acceptHeader: context.Configuration?.ReadWriteConfiguration?.AcceptHeaderOverride ?? accept,
                                    writeableMediaTypes: context.Configuration?.ReadWriteConfiguration?.WriteableMediaTypes,
                                    writeableFormatters: overrides.Formatters,
                                    formatterType: out formatterType).ConfigureAwait(false);
                            }
                        }

                        if (formatter != null)
                        {
                            isWriteableResponse = true;
                            context.Response.ResponseWriter = formatter;
                            context.Response.ResponseWriterOptions = options;
                            context.Response.ContentType = formatterType;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 304;
                    }
                }

                if (!isWriteableResponse)
                {
                    if (context.Response.HasSuccessStatus() && context.Response.StatusCode != 202 && !(context.Runtime?.Internals?.IsOverridingStatusCode ?? false))
                    {
                        context.Response.StatusCode = 204;
                    }
                }
            }

            return true;
        }
    }
}
