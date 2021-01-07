namespace DeepSleep.Pipeline
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestCanceledPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestCanceledPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestCanceledPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            if (await context.ProcessHttpRequestCanceled().ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestCanceledPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request canceled.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestCanceled(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestCanceledPipelineComponent>();
        }

        /// <summary>Processes the HTTP request canceled.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpRequestCanceled(this ApiRequestContext context)
        {
            if (context.RequestAborted.IsCancellationRequested)
            {
                //logger?.LogInformation($"Request has been cancelled by client, issuing HTTP 408 Request Timeout");

                context.Response.StatusCode = 408;

                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
