namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseHttpCachingPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseHttpCachingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseHttpCachingPipelineComponent(ApiRequestDelegate next)
             : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            await context.ProcessHttpResponseCaching().ConfigureAwait(false);
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
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseCaching(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var statusCode = context.Response.StatusCode;

                if (statusCode >= 200 && statusCode <= 299 && statusCode != 204)
                {
                    // Don't add cache headers for pre-flight requests
                    // This is handled in the prflight pipeline component
                    if (context.Request.IsCorsPreflightRequest())
                    {
                        return Task.FromResult(true);
                    }

                    var method = context.Request?.Method?.ToLower() ?? string.Empty;

                    if (method.In(StringComparison.InvariantCultureIgnoreCase, "get", "put", "options", "head"))
                    {
                        var directive = context.Configuration?.CacheDirective;

                        if (directive != null && directive.Cacheability == HttpCacheType.Cacheable && directive.ExpirationSeconds > 0)
                        {
                            context.Response.AddHeader("Cache-Control", $"{(directive.CacheLocation ?? HttpCacheLocation.Private).ToString().ToLower()}, max-age={directive.ExpirationSeconds}");
                            context.Response.AddHeader("Expires", DateTime.UtcNow.AddSeconds(directive.ExpirationSeconds.Value).ToString("r"));

                            // ADDING VARY HEADERS TO SPECIFY WHAT THE RESPONSE REPRESENTATION WAS GENERATED AGAINST.
                            context.Response.AddHeader(
                                name: "Vary", 
                                value: "Accept, Accept-Encoding, Accept-Language", 
                                append: true);

                            return Task.FromResult(true);
                        }
                    }
                }

                context.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate, max-age=0");

                // this gets updated when the response date is added to the headers.  THe value will
                // ultimately be the response date - 1 year.  Needs to be here though because the header is checked
                // for prior to updating it.
                context.Response.AddHeader("Expires", DateTime.UtcNow.AddYears(-1).ToString("r"));
            }

            return Task.FromResult(true);
        }
    }
}
