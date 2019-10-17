namespace DeepSleep.Pipeline
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestEndpointInvocationPipelineComponent : PipelineComponentBase
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestEndpointInvocationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestEndpointInvocationPipelineComponent(ApiRequestDelegate next)
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
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestEndpointInvocationPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestEndpointInvocationPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;
            bool canContinuePipeline = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.RequestEndpointInvocationPipeline, ApiRequestPipelineHookPlacements.Before).ConfigureAwait(false);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }

            if (canInvokeComponent)
            {
                if (!await context.ProcessHttpEndpointInvocation().ConfigureAwait(false))
                    canContinuePipeline = false;
            }

            if (afterHook != null)
            {
                var result = await afterHook.Hook(context, ApiRequestPipelineComponentTypes.RequestEndpointInvocationPipeline, ApiRequestPipelineHookPlacements.After).ConfigureAwait(false);
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
    public static class ApiRequestEndpointInvocationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request endpoint invocation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestEndpointInvocation(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestEndpointInvocationPipelineComponent>();
        }
    }
}
