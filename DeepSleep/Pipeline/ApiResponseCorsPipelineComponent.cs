namespace DeepSleep.Pipeline
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCorsPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorsPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCorsPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }


        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            finally
            {
                var context = contextResolver.GetContext();
                await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseCorsPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response cors.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseCors(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseCorsPipelineComponent>();
        }

        /// <summary>Processes the HTTP response cross origin resource sharing.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseCrossOriginResourceSharing(this ApiRequestContext context)
        {
            if (!(context?.RequestAborted.IsCancellationRequested ?? false))
            {
                if (!string.IsNullOrWhiteSpace(context.RequestInfo?.CrossOriginRequest?.Origin))
                {
                    var allowedOrigins = context.RequestConfig?.CrossOriginConfig?.AllowedOrigins;
                    var exposeHeaders = context.RequestConfig?.CrossOriginConfig?.ExposeHeaders;
                    var allowCredentials = context.RequestConfig?.CrossOriginConfig?.AllowCredentials;

                    string allowedOrigin = null;

                    if ((allowedOrigins?.Count() ?? 0) > 0 && allowedOrigins.Contains("*"))
                    {
                        allowedOrigin = context.RequestInfo.CrossOriginRequest.Origin;
                    }
                    else
                    {
                        allowedOrigin = (allowedOrigins ?? new string[] { })
                            .Distinct()
                            .Where(i => !string.IsNullOrWhiteSpace(i))
                            .Select(i => i.Trim())
                            .Where(i => i.Equals(context.RequestInfo.CrossOriginRequest.Origin, StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault();
                    }

                    context.ResponseInfo.AddHeader("Access-Control-Allow-Origin", allowedOrigin ?? string.Empty);
                    context.ResponseInfo.AddHeader("Access-Control-Allow-Credentials", (allowCredentials ?? false).ToString().ToLowerInvariant());

                    if ((exposeHeaders?.Count() ?? 0) > 0)
                    {
                        var headers = exposeHeaders
                            .Distinct()
                            .Where(i => !string.IsNullOrWhiteSpace(i))
                            .Select(i => i.Trim());

                        context.ResponseInfo.AddHeader("Access-Control-Expose-Headers", string.Join(", ", headers));
                    }
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
