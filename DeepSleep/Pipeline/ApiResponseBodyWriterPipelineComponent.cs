using DeepSleep.Formatting;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseBodyWriterPipelineComponent
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseBodyWriterPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseBodyWriterPipelineComponent(ApiRequestDelegate next)
        {
            _apinext = next;
        }

        private readonly ApiRequestDelegate _apinext;

        #endregion

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="formatterFactory">The formatter factory.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiServiceConfiguration config, IFormatStreamReaderWriterFactory formatterFactory)
        {
            await _apinext.Invoke(contextResolver);

            var context = contextResolver.GetContext();
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseBodyWriterPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseBodyWriterPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseBodyWriterPipeline, ApiRequestPipelineHookPlacements.Before);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
            }


            if (canInvokeComponent)
            {
                await context.ProcessHttpResponseBodyWriting(formatterFactory);
            }


            if (afterHook != null)
            {
                await afterHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseBodyWriterPipeline, ApiRequestPipelineHookPlacements.After);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseBodyWriterPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response body writer.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseBodyWriter(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseBodyWriterPipelineComponent>();
        }
    }
}
