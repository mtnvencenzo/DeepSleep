namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseRequestProcessingPipelineComponent : PipelineComponentBase
    {
        /// <summary>Initializes a new instance of the <see cref="ApiResponseRequestProcessingPipelineComponent"/> class.</summary>
        /// <param name="next">The next.</param>
        public ApiResponseRequestProcessingPipelineComponent(ApiRequestDelegate next)
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

            var apiServiceConfiguration = context?.RequestServices?.GetService<IApiServiceConfiguration>();
            
            await context.ProcessHttpResponseRequestProcessing(apiServiceConfiguration).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseRequestProcessedPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response request processing.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseRequestProcessing(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseRequestProcessingPipelineComponent>();
        }

        /// <summary>Processes the HTTP response request processing.</summary>
        /// <param name="context">The context.</param>
        /// <param name="apiServiceConfiguration">The API service configuration.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseRequestProcessing(this ApiRequestContext context, IApiServiceConfiguration apiServiceConfiguration)
        {
            if (apiServiceConfiguration?.OnRequestProcessed != null)
            {
                try
                {
                    await apiServiceConfiguration.OnRequestProcessed(context).ConfigureAwait(false);
                }
                catch { }
            }

            return true;
        }
    }
}
