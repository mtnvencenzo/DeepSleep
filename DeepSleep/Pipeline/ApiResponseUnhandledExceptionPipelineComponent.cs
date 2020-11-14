namespace DeepSleep.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Resources;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseUnhandledExceptionPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseUnhandledExceptionPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseUnhandledExceptionPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiServiceConfiguration config, IApiResponseMessageConverter responseMessageConverter, ILogger<ApiResponseUnhandledExceptionPipelineComponent> logger)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var context = contextResolver.GetContext();

                await context.ProcessHttpResponseUnhandledException(ex, config, responseMessageConverter, logger).ConfigureAwait(false);
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
        /// <param name="exception">The exception.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseUnhandledException(this ApiRequestContext context, Exception exception, IApiServiceConfiguration config, IApiResponseMessageConverter responseMessageConverter, ILogger logger)
        {
            if (exception != null)
            {
                var code = context.HandleException(exception, responseMessageConverter);

                if (config?.ExceptionHandler != null && exception as ApiNotImplementedException == null)
                {
                    try
                    {
                        await config.ExceptionHandler(context, exception).ConfigureAwait(false);
                    }
                    catch (Exception ex) 
                    {
                        logger?.LogError(ex, $"Failed calling exception handler");
                        logger?.LogError(exception, "Recorded exception");
                    }
                }

                logger?.LogWarning($"Excetion recorded, issueing HTTP {code}");

                context.ResponseInfo.ResponseObject = new ApiResponse
                {
                    StatusCode = code
                };
            }

            return true;
        }

        /// <summary>Handles the exception.</summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        private static int HandleException(this ApiRequestContext context, Exception exception, IApiResponseMessageConverter responseMessageConverter)
        {
            int code;

            if (exception is ApiNotImplementedException)
            {
                code = 501;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.NotImplemented));
                    context.AddException(exception);
                }
            }
            else if (exception is ApiBadGatewayException)
            {
                code = 502;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.BadGateway));
                    context.AddException(exception);
                }
            }
            else if (exception is ApiServiceUnavailableException)
            {
                code = 503;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.ServiceUnavailable));
                    context.AddException(exception);
                }
            }
            else if (exception is ApiGatewayTimeoutyException)
            {
                code = 504;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.GatewayTimeout));
                    context.AddException(exception);
                }
            }
            else
            {
                code = 500;

                if (context != null)
                {
                    context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(UnhandledExceptionErrors.UnhandledException));
                    context.AddException(exception);
                }
            }

            return code;
        }
    }
}
