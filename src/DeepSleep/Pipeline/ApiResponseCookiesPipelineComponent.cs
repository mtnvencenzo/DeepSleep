namespace DeepSleep.Pipeline
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCookiesPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorsPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCookiesPipelineComponent(ApiRequestDelegate next)
            : base(next) { }


        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            finally
            {
                var context = contextResolver
                     .GetContext()
                     .SetThreadCulure();

                await context.ProcessHttpResponseCookies().ConfigureAwait(false);
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
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseCookies(this ApiRequestContext context)
        {
            if (!(context?.RequestAborted.IsCancellationRequested ?? false))
            {
                if (context.Response != null && context.Response.Cookies != null)
                {
                    foreach (var cookie in context.Response.Cookies)
                    {
                        context.Response.AddHeader("Set-Cookie", cookie.ToCookie());
                    };
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(true);
        }
    }
}
