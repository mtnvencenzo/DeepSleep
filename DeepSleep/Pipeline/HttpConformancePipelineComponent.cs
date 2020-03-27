namespace DeepSleep.Pipeline
{
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 
    /// </summary>
    public class HttpConformancePipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpConformancePipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public HttpConformancePipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<HttpConformancePipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpConformance(logger).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiHttpConformancePipelineComponentExtensionMethods
    {
        /// <summary>Uses the API HTTP comformance.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiHttpComformance(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<HttpConformancePipelineComponent>();
        }

        /// <summary>Processes the HTTP conformance.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpConformance(this ApiRequestContext context, ILogger logger)
        {
            logger?.LogInformation("Invoked");

            if (!context.RequestAborted.IsCancellationRequested)
            {
                var validHttpVersions = (context?.RequestConfig?.HttpConfig?.SupportedVersions ?? new string[] { "*" })
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .Select(i => i.ToLowerInvariant())
                    .ToList();

                if (validHttpVersions.Contains("*"))
                {
                    return Task.FromResult(true);
                }

                // Only supportting http 1.1 and http 2.0
                if (!validHttpVersions.Contains(context?.RequestInfo?.Protocol?.ToLowerInvariant()))
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 505
                    };

                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
