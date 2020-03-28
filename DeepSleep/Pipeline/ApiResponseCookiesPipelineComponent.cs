namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCookiesPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorsPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCookiesPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }


        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiResponseCookiesPipelineComponent> logger)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            finally
            {
                var context = contextResolver.GetContext();
                await context.ProcessHttpResponseCookies(logger).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseCookiesPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response cors.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseCookies(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseCookiesPipelineComponent>();
        }

        /// <summary>Processes the HTTP response cross origin resource sharing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static Task<bool> ProcessHttpResponseCookies(this ApiRequestContext context, ILogger logger)
        {
            if (!(context?.RequestAborted.IsCancellationRequested ?? false))
            {
                if (context.ResponseInfo != null && context.ResponseInfo.Cookies != null)
                {
                    foreach(var cookie in context.ResponseInfo.Cookies)
                    {
                        context.ResponseInfo.AddHeader("Set-Cookie", cookie.ToCookie());
                    };
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(true);
        }
    }
}
