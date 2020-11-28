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
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormatStreamReaderWriterFactory formatterFactory)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

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
        public static async Task<bool> ProcessHttpResponseBodyWriting(this ApiRequestContext context, IFormatStreamReaderWriterFactory formatterFactory)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var isWriteableResponse = false;

                if (context.ResponseInfo.ResponseObject != null && formatterFactory != null)
                {
                    var accept = !string.IsNullOrWhiteSpace(context.RequestInfo.Accept)
                        ? context.RequestInfo.Accept
                        : new MediaHeaderValueWithQualityString("*/*");

                    var formatter = await formatterFactory.GetAcceptableFormatter(accept, out var formatterType).ConfigureAwait(false);

                    if (formatter != null)
                    {
                        var isConditionalRequestMatch = ApiCondtionalMatchType.None;

                        if (string.Equals(context.RequestInfo.Method, "get", System.StringComparison.OrdinalIgnoreCase) && context.RequestInfo.IsHeadRequest() == false)
                        {
                            isConditionalRequestMatch = context.IsConditionalRequestMatch(context.ResponseInfo);
                        }

                        if (isConditionalRequestMatch != ApiCondtionalMatchType.ConditionalGetMatch)
                        {
                            var formatterOptions = context.ProcessingInfo.OverridingFormatOptions ?? new FormatterOptions { PrettyPrint = context.RequestInfo.PrettyPrint };

                            if (formatter.SupportsPrettyPrint && context.RequestInfo.PrettyPrint)
                            {
                                context.ResponseInfo.AddHeader("X-PrettyPrint", formatterOptions.PrettyPrint.ToString().ToLower());
                            }

                            isWriteableResponse = true;
                            context.ResponseInfo.ResponseWriter = formatter;
                            context.ResponseInfo.ResponseWriterOptions = formatterOptions;
                            context.ResponseInfo.ContentType = formatterType;
                        }
                        else
                        {
                            context.ResponseInfo.StatusCode = 304;
                        }
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
