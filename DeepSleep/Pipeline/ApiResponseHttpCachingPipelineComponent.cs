namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseHttpCachingPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseHttpCachingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseHttpCachingPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiResponseHttpCachingPipelineComponent> logger)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            await context.ProcessHttpResponseCaching(logger).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseHttpCachingPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response HTTP caching.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseHttpCaching(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseHttpCachingPipelineComponent>();
        }

        /// <summary>Processes the HTTP response caching.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseCaching(this ApiRequestContext context, ILogger logger)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var statusCode = context.ResponseInfo.StatusCode;

                if (statusCode >= 200 && statusCode <= 299 && statusCode != 204)
                {
                    if (context.RequestInfo.IsCorsPreflightRequest())
                    {
                        context.ResponseInfo.AddHeader("Vary", "Origin");
                        context.ResponseInfo.AddHeader("Access-Control-Max-Age", "600");

                        return Task.FromResult(true);
                    }

                    var method = context.RequestInfo?.Method?.ToLower() ?? string.Empty;

                    if (method.In(StringComparison.InvariantCultureIgnoreCase, "get", "put", "options", "head"))
                    {
                        var directive = context.RequestConfig?.CacheDirective;

                        if (directive != null && directive.Cacheability == HttpCacheType.Cacheable && directive.ExpirationSeconds > 0)
                        {
                            context.ResponseInfo.AddHeader("Cache-Control", $"{(directive.CacheLocation ?? HttpCacheLocation.Private).ToString().ToLower()}, max-age={directive.ExpirationSeconds}");
                            context.ResponseInfo.AddHeader("Expires", DateTime.UtcNow.AddSeconds(directive.ExpirationSeconds.Value).ToString("r"));

                            // ADDING VARY HEADERS TO SPECIFY WHAT THE RESPONSE REPRESENTATION WAS GENERATED AGAINST.
                            context.ResponseInfo.AddHeader("Vary", "Accept, Accept-Encoding, Accept-Language");

                            return Task.FromResult(true);
                        }
                    }
                }

                context.ResponseInfo.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate, max-age=0");

                // this gets updated when the response date is added to the headers.  THe value will
                // ultimately be the response date - 1 year.  Needs to be here though because the header is checked
                // for prior to updating it.
                context.ResponseInfo.AddHeader("Expires", DateTime.UtcNow.AddYears(-1).ToString("r")); 
            }

            return Task.FromResult(true);
        }
    }
}
