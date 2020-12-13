namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
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
                        : new MediaHeaderValueWithQualityString("*/*");

                    IFormatStreamReaderWriter formatter = null;

                    if (formatterFactory != null)
                    {
                        formatter = await formatterFactory.GetAcceptableFormatter(accept, out var _).ConfigureAwait(false);
                    }

                    if (formatter == null)
                    {
                        string acceptable = (formatterFactory != null)
                            ? string.Join(", ", formatterFactory.GetTypes())
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
