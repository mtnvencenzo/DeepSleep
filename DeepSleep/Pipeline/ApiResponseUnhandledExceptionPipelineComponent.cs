﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep.Pipeline
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseUnhandledExceptionPipelineComponent
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseUnhandledExceptionPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseUnhandledExceptionPipelineComponent(ApiRequestDelegate next)
        {
            _apinext = next;
        }

        private readonly ApiRequestDelegate _apinext;

        #endregion

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiServiceConfiguration config, IApiResponseMessageConverter responseMessageConverter)
        {
            Exception caught = null;

            try
            {
                await _apinext.Invoke(contextResolver);
            }
            catch (System.Exception ex)
            {
                caught = ex;
            }

            if (caught == null)
                return;


            var context = contextResolver.GetContext();
            var beforeHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseUnhandledExceptionPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.Before));
            var afterHook = config.GetPipelineHooks(ApiRequestPipelineComponentTypes.ResponseUnhandledExceptionPipeline).FirstOrDefault(h => h.Placements.HasFlag(ApiRequestPipelineHookPlacements.After));

            bool canInvokeComponent = true;

            if (beforeHook != null)
            {
                var result = await beforeHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseUnhandledExceptionPipeline, ApiRequestPipelineHookPlacements.Before);
                if (result.Continuation == ApiRequestPipelineHookContinuation.ByPassComponentAndCancel || result.Continuation == ApiRequestPipelineHookContinuation.BypassComponentAndContinue)
                    canInvokeComponent = false;
            }


            if (canInvokeComponent)
            {
                await context.ProcessHttpResponseUnhandledException(caught, config, responseMessageConverter);
            }


            if (afterHook != null)
            {
                await afterHook.Hook(context, ApiRequestPipelineComponentTypes.ResponseUnhandledExceptionPipeline, ApiRequestPipelineHookPlacements.After);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseUnhandledExceptionPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response unhandled exception handler.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseUnhandledExceptionHandler(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseUnhandledExceptionPipelineComponent>();
        }
    }
}
