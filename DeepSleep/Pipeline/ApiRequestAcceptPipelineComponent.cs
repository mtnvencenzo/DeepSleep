namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestAcceptPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAcceptPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAcceptPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified formatter factory.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            var formatterFactory = context?.RequestServices?.GetService<IFormatStreamReaderWriterFactory>();

            if (await context.ProcessHttpRequestAccept(formatterFactory).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestAcceptPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request accept.</summary>
        /// <param name="pipline">The pipline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestAccept(this IApiRequestPipeline pipline)
        {
            return pipline.UsePipelineComponent<ApiRequestAcceptPipelineComponent>();
        }

        /// <summary>Processes the HTTP request accept.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestAccept(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.RequestInfo.Accept)
                        ? context.RequestInfo.Accept
                        : AcceptHeader.All();

                    IFormatStreamReaderWriter formatter = null;

                    if (formatterFactory != null)
                    {
                        formatter = await formatterFactory.GetAcceptableFormatter(
                            acceptHeader: context.RequestConfig?.ReadWriteConfiguration?.AcceptHeaderOverride ?? accept,
                            writeableMediaTypes: context.RequestConfig?.ReadWriteConfiguration?.WriteableMediaTypes,
                            formatterType: out var _).ConfigureAwait(false);
                    }

                    IList<IFormatStreamReaderWriter> overridingFormatters = null;

                    if (context.RequestConfig.ReadWriteConfiguration?.WriterResolver != null)
                    {
                        var overrides = await context.RequestConfig.ReadWriteConfiguration.WriterResolver(new ResolvedFormatterArguments(context, formatter, null)).ConfigureAwait(false);

                        overridingFormatters = overrides?.Formatters;

                        if (overrides?.Formatters != null)
                        {
                            formatter = await formatterFactory.GetAcceptableFormatter(
                                acceptHeader: context.RequestConfig?.ReadWriteConfiguration?.AcceptHeaderOverride ?? accept,
                                writeableMediaTypes: context.RequestConfig?.ReadWriteConfiguration?.WriteableMediaTypes,
                                writeableFormatters: overrides.Formatters,
                                formatterType: out var _).ConfigureAwait(false);
                        }
                    }


                    if (formatter == null)
                    {
                        var formatterTypes = formatterFactory != null
                            ? formatterFactory.GetWriteableTypes(overridingFormatters) ?? new List<string>()
                            : new HttpMediaTypeStreamReaderWriterFactory(context.RequestServices).GetWriteableTypes(overridingFormatters) ?? new List<string>();

                        var writeableMediaTypes = context.RequestConfig?.ReadWriteConfiguration?.WriteableMediaTypes ?? formatterTypes ?? new List<string>();

                        var acceptableTypes = writeableMediaTypes
                            .Where(w => formatterTypes.Any(f => string.Equals(f, w, System.StringComparison.InvariantCultureIgnoreCase)))
                            .ToList();

                        string acceptable = (acceptableTypes != null && acceptableTypes.Count > 0)
                            ? string.Join(", ", acceptableTypes)
                            : string.Empty;

                        context.ResponseInfo.AddHeader("X-Allow-Accept", acceptable);
                        context.ResponseInfo.StatusCode = 406;
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
