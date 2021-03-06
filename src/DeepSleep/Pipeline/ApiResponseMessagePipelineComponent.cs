﻿namespace DeepSleep.Pipeline
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseMessagePipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseMessagePipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseMessagePipelineComponent(ApiRequestDelegate next)
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

            await context.ProcessHttpResponseMessages(contextResolver).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseMessagePipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response messages.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseMessages(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseMessagePipelineComponent>();
        }

        /// <summary>Processes the HTTP response messages.</summary>
        /// <param name="context">The context.</param>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpResponseMessages(this ApiRequestContext context, IApiRequestContextResolver contextResolver)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Response != null && context.Response.ResponseObject == null && context.Response.HasSuccessStatus() == false)
                {
                    if (context.Configuration?.ApiErrorResponseProvider != null)
                    {
                        var provider = context.Configuration.ApiErrorResponseProvider(context.RequestServices);

                        if (provider != null)
                        {
                            var errors = context.Validation?.Errors ?? new List<string>();

                            context.Response.ResponseObject = await provider.Process(contextResolver, errors).ConfigureAwait(false);
                        }
                    }
                }
            }

            return true;
        }
    }
}
