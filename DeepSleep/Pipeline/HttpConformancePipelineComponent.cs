﻿namespace DeepSleep.Pipeline
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class HttpConformancePipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpConformancePipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public HttpConformancePipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpConformance().ConfigureAwait(false))
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
        /// <returns></returns>
        internal static Task<bool> ProcessHttpConformance(this ApiRequestContext context)
        {
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
                    //logger?.LogInformation($"Http version {context.RequestInfo?.Protocol} is un-supported, issueing HTTP 505 HTTP Version Not Supported");

                    context.ResponseInfo.StatusCode = 505;

                    return Task.FromResult(false);
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
