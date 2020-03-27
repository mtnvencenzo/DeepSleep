namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using Microsoft.Extensions.Logging;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseBodyWriterPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseBodyWriterPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseBodyWriterPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormatStreamReaderWriterFactory formatterFactory, ILogger<ApiResponseBodyWriterPipelineComponent> logger)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            await context.ProcessHttpResponseBodyWriting(formatterFactory, logger).ConfigureAwait(false);
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
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseBodyWriting(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory, ILogger logger)
        {
            logger?.LogInformation("Invoked");

            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.ResponseInfo.ResponseObject == null)
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse();
                }

                if (context.ResponseInfo.ResponseObject?.Body != null && formatterFactory != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.RequestInfo.Accept)
                        ? context.RequestInfo.Accept
                        : new MediaHeaderValueWithQualityString("*/*");

                    var formatter = await formatterFactory.GetAcceptableFormatter(accept, out var formatterType).ConfigureAwait(false);

                    if (formatter != null)
                    {
                        var formatterOptions = (context.ProcessingInfo.OverridingFormatOptions != null)
                            ? context.ProcessingInfo.OverridingFormatOptions
                            : new FormatterOptions { PrettyPrint = context.RequestInfo.PrettyPrint };

                        if (formatter.SupportsPrettyPrint && context.RequestInfo.PrettyPrint)
                        {
                            context.ResponseInfo.AddHeader("X-PrettyPrint", formatterOptions.PrettyPrint.ToString().ToLower());
                        }

                        context.ResponseInfo.ContentType = formatterType;

                        using (var m = new MemoryStream())
                        {
                            await formatter.WriteType(m, context.ResponseInfo.ResponseObject.Body, formatterOptions).ConfigureAwait(false);

                            context.ResponseInfo.ContentLength = m.Length;
                            context.ResponseInfo.RawResponseObject = m.ToArray();
                        }
                    }
                }

                if (context.ResponseInfo.ContentLength == 0 && context.ResponseInfo.RawResponseObject == null)
                {
                    if (context.ResponseInfo.HasSuccessStatus() && (context.ResponseInfo.ResponseObject?.StatusCode ?? 200) != 202)
                    {
                        context.ResponseInfo.ResponseObject.StatusCode = 204;
                    }
                }
            }

            return true;
        }
    }
}
