namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseUnhandledExceptionPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseUnhandledExceptionPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseUnhandledExceptionPipelineComponent(ApiRequestDelegate next)
              : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var context = contextResolver
                    .GetContext()
                    .SetThreadCulure();

                var apiServiceConfiguration = context?.RequestServices?.GetService<IApiServiceConfiguration>();

                await context.ProcessHttpResponseUnhandledException(contextResolver, ex, apiServiceConfiguration).ConfigureAwait(false);
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

        /// <summary>Processes the HTTP response unhandled exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseUnhandledException(this ApiRequestContext context, IApiRequestContextResolver contextResolver, Exception exception, IApiServiceConfiguration config)
        {
            if (exception != null)
            {
                var code = context.HandleException(exception);


                if (config?.OnException != null && exception as ApiException == null)
                {
                    try
                    {
                        await config.OnException(contextResolver, exception).ConfigureAwait(false);
                    }
                    catch { }
                }

                context.Response.StatusCode = code;
            }

            return true;
        }

        /// <summary>Handles the exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        internal static int HandleException(this ApiRequestContext context, Exception exception)
        {
            int code;

            if (exception is ApiException apiException)
            {
                code = apiException.HttpStatus;
            }
            else
            {
                code = 500;
            }

            if (context != null)
            {
                context.AddException(exception);
            }

            return code;
        }
    }
}
