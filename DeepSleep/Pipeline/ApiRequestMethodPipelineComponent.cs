namespace DeepSleep.Pipeline
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestMethodPipelineComponent : PipelineComponentBase
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestMethodPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestMethodPipelineComponent(ApiRequestDelegate next)
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
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestMethodPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestMethodPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;
            bool canContinuePipeline = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.RequestMethodPipeline, ApiRequestPipelineHookPlacements.Before).ConfigureAwait(false);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }

            if (canInvokeComponent)
            {
                if (!await context.ProcessHttpRequestMethod().ConfigureAwait(false))
                    canContinuePipeline = false;
            }

            if (afterHook != null)
            {
                var result = await afterHook.Hook(context, ApiRequestPipelineComponentTypes.RequestMethodPipeline, ApiRequestPipelineHookPlacements.After).ConfigureAwait(false);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }


            if (canContinuePipeline)
                await _apinext.Invoke(contextResolver).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestMethodPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request method.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestMethod(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestMethodPipelineComponent>();
        }
    }
}
