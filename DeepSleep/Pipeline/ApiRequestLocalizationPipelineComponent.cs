namespace DeepSleep.Pipeline
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestLocalizationPipelineComponent : PipelineComponentBase
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestLocalizationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestLocalizationPipelineComponent(ApiRequestDelegate next)
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
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestLocalizationPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.RequestLocalizationPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;
            bool canContinuePipeline = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.RequestLocalizationPipeline, ApiRequestPipelineHookPlacements.Before).ConfigureAwait(false);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.InvokeComponentAndCancel)
                    canContinuePipeline = false;
            }

            if (canInvokeComponent)
            {
                if (!await context.ProcessHttpRequestLocalization().ConfigureAwait(false))
                    canContinuePipeline = false;
            }

            if (afterHook != null)
            {
                var result = await afterHook.Hook(context, ApiRequestPipelineComponentTypes.RequestLocalizationPipeline, ApiRequestPipelineHookPlacements.After).ConfigureAwait(false);
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
    public static class ApiRequestLocalizationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request localization.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestLocalization(this IApiRequestPipeline pipeline)
        {
            //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            return pipeline.UsePipelineComponent<ApiRequestLocalizationPipelineComponent>();
        }
    }
}
