namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestAcceptPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAcceptPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAcceptPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified formatter factory.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormatStreamReaderWriterFactory formatterFactory)
        {
            var context = contextResolver.GetContext();

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
        public static async Task<bool> ProcessHttpRequestAccept(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
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
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 406
                        };
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
