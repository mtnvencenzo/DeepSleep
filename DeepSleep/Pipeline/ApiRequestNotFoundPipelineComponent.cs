using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestNotFoundPipelineComponent
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestNotFoundPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestNotFoundPipelineComponent(ApiRequestDelegate next)
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
            var context = contextResolver.GetContext();
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestNotFoundPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestNotFoundPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;
            bool canContinuePipeline = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.RequestNotFoundPipeline, ApiRequestPipelineHookPlacements.Before);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }

            if (canInvokeComponent)
            {
                if (!await context.ProcessHttpRequestNotFound())
                    canContinuePipeline = false;
            }

            if (afterHook != null)
            {
                var result = await afterHook.Hook(context, ApiRequestPipelineComponentTypes.RequestNotFoundPipeline, ApiRequestPipelineHookPlacements.After);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }


            if (canContinuePipeline)
                await _apinext.Invoke(contextResolver);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestNotFoundPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request not found.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestNotFound(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestNotFoundPipelineComponent>();
        }
    }
}
