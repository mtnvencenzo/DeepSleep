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

            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var defaultRequestConfiguration = context.RequestServices.GetService<IApiRequestConfiguration>();

            await context.ProcessHttpResponseCaching(defaultRequestConfiguration).ConfigureAwait(false);
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
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseCaching(this ApiRequestContext context, IApiRequestConfiguration defaultRequestConfiguration)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var statusCode = context.Response.StatusCode;
                var systemConfiguration = ApiRequestContext.GetDefaultRequestConfiguration();

                var expirationSeconds = context.Configuration?.CacheDirective?.ExpirationSeconds
                    ?? defaultRequestConfiguration?.CacheDirective?.ExpirationSeconds
                    ?? systemConfiguration.CacheDirective.ExpirationSeconds.Value;

                var cacheability = context.Configuration?.CacheDirective?.Cacheability
                    ?? defaultRequestConfiguration?.CacheDirective?.Cacheability
                    ?? systemConfiguration.CacheDirective.Cacheability.Value;

                var cacheLocation = context.Configuration?.CacheDirective?.CacheLocation
                    ?? defaultRequestConfiguration?.CacheDirective?.CacheLocation
                    ?? systemConfiguration.CacheDirective.CacheLocation.Value;

                var vary = context.Configuration?.CacheDirective?.VaryHeaderValue
                    ?? defaultRequestConfiguration?.CacheDirective?.VaryHeaderValue
                    ?? systemConfiguration.CacheDirective.VaryHeaderValue;

                if (statusCode >= 200 && statusCode <= 299 && statusCode != 204)
                {
                    // Don't add cache headers for pre-flight requests
                    // This is handled in the prflight pipeline component
                    if (context.Request.IsCorsPreflightRequest())
                    {
                        return Task.FromResult(true);
                    }

                    if (cacheability == HttpCacheType.Cacheable)
                    {
                        var maxAgeSeconds = expirationSeconds < 0
                            ? 0
                            : expirationSeconds;

                        context.Response.AddHeader("Cache-Control", $"{cacheLocation.ToString().ToLower()}, max-age={maxAgeSeconds}");
                        context.Response.AddHeader("Expires", DateTime.UtcNow.AddSeconds(expirationSeconds).ToString("r"));

                        if (!string.IsNullOrWhiteSpace(vary))
                        {
                            // ADDING VARY HEADERS TO SPECIFY WHAT THE RESPONSE REPRESENTATION WAS GENERATED AGAINST.
                            context.Response.AddHeader(
                                name: "Vary",
                                value: vary,
                                append: true);
                        }

                        return Task.FromResult(true);
                    }
                }

                var seconds = expirationSeconds > 0 
                    ? -1 
                    : expirationSeconds;

                context.Response.AddHeader("Cache-Control", $"no-store, max-age=0");

                // this gets updated when the response date is added to the headers.  THe value will
                // ultimately be the response date - expiration seconds.  Needs to be here though because the header is checked
                // for prior to updating it.
                context.Response.AddHeader("Expires", DateTime.UtcNow.AddSeconds(seconds).ToString("r"));
            }

            return Task.FromResult(true);
        }
    }
}
