namespace DeepSleep.Pipeline
{
    using DeepSleep.Formatting;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestAcceptPipelineComponent : PipelineComponentBase
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAcceptPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAcceptPipelineComponent(ApiRequestDelegate next)
        {
            _apinext = next;
        }

        private readonly ApiRequestDelegate _apinext;

        #endregion

        /// <summary>Invokes the specified formatter factory.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IFormatStreamReaderWriterFactory formatterFactory, IApiServiceConfiguration config)
        {
            var context = contextResolver.GetContext();
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestAcceptPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestAcceptPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;
            bool canContinuePipeline = true;


            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.RequestAcceptPipeline, ApiRequestPipelineHookPlacements.Before).ConfigureAwait(false);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }

            if (canInvokeComponent)
            {
                if (!await context.ProcessHttpRequestAccept(formatterFactory).ConfigureAwait(false))
                    canContinuePipeline = false;
            }

            if (afterHook != null)
            {
                var result = await afterHook.Hook(context, ApiRequestPipelineComponentTypes.RequestAcceptPipeline, ApiRequestPipelineHookPlacements.After).ConfigureAwait(false);
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
    public static class ApiRequestAcceptPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request accept.</summary>
        /// <param name="pipline">The pipline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestAccept(this IApiRequestPipeline pipline)
        {
            return pipline.UsePipelineComponent<ApiRequestAcceptPipelineComponent>();
        }
    }
}
