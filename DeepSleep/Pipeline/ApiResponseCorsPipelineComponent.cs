using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCorsPipelineComponent
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorsPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCorsPipelineComponent(ApiRequestDelegate next)
        {
            _apinext = next;
        }

        private readonly ApiRequestDelegate _apinext;

        #endregion

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="corsConfigResolver">The cors configuration resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiServiceConfiguration config, ICrossOriginConfigResolver corsConfigResolver)
        {
            try
            {
                await _apinext.Invoke(contextResolver);
            }
            finally
            {
                var context = contextResolver.GetContext();
                var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseCorsPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
                var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseCorsPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

                bool canInvokeComponent = true;

                if (beforeHook != null)
                {
                    var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseCorsPipeline, ApiRequestPipelineHookPlacements.Before);
                    if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                        canInvokeComponent = false;
                }


                if (canInvokeComponent)
                {
                    await context.ProcessHttpResponseCrossOriginResourceSharing(corsConfigResolver);
                }


                if (afterHook != null)
                {
                    await afterHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseCorsPipeline, ApiRequestPipelineHookPlacements.After);
                }
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
    }
}
