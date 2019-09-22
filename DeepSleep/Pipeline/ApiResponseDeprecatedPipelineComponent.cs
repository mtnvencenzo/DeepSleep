using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseDeprecatedPipelineComponent
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="ApiResponseDeprecatedPipelineComponent"/> class.</summary>
        /// <param name="next">The next.</param>
        public ApiResponseDeprecatedPipelineComponent(ApiRequestDelegate next)
        {
            _apinext = next;
        }


        private readonly ApiRequestDelegate _apinext;

        #endregion

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiServiceConfiguration config)
        {
            await _apinext.Invoke(contextResolver);

            var context = contextResolver.GetContext();
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseDeprecatedPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseDeprecatedPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseDeprecatedPipeline, ApiRequestPipelineHookPlacements.Before);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
            }


            if (canInvokeComponent)
            {
                await context.ProcessHttpResponseDeprecated();
            }


            if (afterHook != null)
            {
                await afterHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseDeprecatedPipeline, ApiRequestPipelineHookPlacements.After);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseDeprecatedPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response correlation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseDeprecated(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseDeprecatedPipelineComponent>();
        }
    }
}
