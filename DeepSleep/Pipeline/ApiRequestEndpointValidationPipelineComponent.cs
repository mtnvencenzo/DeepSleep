﻿namespace DeepSleep.Pipeline
{
    using DeepSleep.Validation;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestEndpointValidationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestEndpointValidationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestEndpointValidationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="validationProvider">The validation provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiValidationProvider validationProvider, IApiResponseMessageConverter responseMessageConverter, ILogger<ApiRequestEndpointValidationPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpEndpointValidation(validationProvider, context.RequestServices, responseMessageConverter, logger).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestEndpointValidationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request endpoint validation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestEndpointValidation(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestEndpointValidationPipelineComponent>();
        }

        /// <summary>Processes the HTTP endpoint validation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="validationProvider">The validation provider.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpEndpointValidation(this ApiRequestContext context, IApiValidationProvider validationProvider, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter, ILogger logger)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                context.ProcessingInfo.Validation.State = ApiValidationState.Validating;

                if (validationProvider != null)
                {
                    var invokers = validationProvider
                        .GetInvokers()
                        .ToList();

                    logger?.LogInformation($"Validating request using {invokers.Count} validation invokers");

                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo.InvocationContext.UriModel != null)
                        {
                            var objectUriValidationResult = await validationInvoker.InvokeObjectValidation(context.RequestInfo.InvocationContext.UriModel, context, serviceProvider, responseMessageConverter).ConfigureAwait(false);
                            if (!objectUriValidationResult)
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                            }
                        }
                    }


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo.InvocationContext.BodyModel != null)
                        {
                            try
                            {
                                var objectBodyValidationResult = await validationInvoker.InvokeObjectValidation(context.RequestInfo.InvocationContext.BodyModel, context, serviceProvider, responseMessageConverter).ConfigureAwait(false);
                                if (!objectBodyValidationResult)
                                {
                                    context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                                }
                            }
                            catch
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Exception;
                                throw;
                            }
                        }
                    }


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo?.InvocationContext?.ControllerMethod != null)
                        {
                            try
                            {
                                var methodValidationResult = await validationInvoker.InvokeMethodValidation(context.RequestInfo.InvocationContext.ControllerMethod, context, serviceProvider, responseMessageConverter).ConfigureAwait(false);
                                if (!methodValidationResult)
                                {
                                    context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                                }
                            }
                            catch
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Exception;
                                throw;
                            }
                        }
                    }
                }


                if (context.ProcessingInfo.Validation.State == ApiValidationState.Failed)
                {
                    bool has400 = context.ProcessingInfo.ExtendedMessages.Exists(m => m != null && m.Code.StartsWith("400"));
                    bool has404 = context.ProcessingInfo.ExtendedMessages.Exists(m => m != null && m.Code.StartsWith("404"));

                    var statusCode = (has404 && !has400)
                        ? 404
                        : 400;

                    
                    foreach (var message in context.ProcessingInfo.ExtendedMessages.Where(m => m.Code.StartsWith("4") || m.Code.StartsWith("5")))
                    {
                        logger?.LogWarning($"Validation for request {context.RequestInfo.RequestIdentifier} failed with error: {message.Code} - {message.Message}");
                    }

                    context.ResponseInfo.StatusCode = statusCode;

                    return false;
                }

                logger?.LogInformation($"Validation for request {context.RequestInfo.RequestIdentifier} succeeded.");

                context.ProcessingInfo.Validation.State = ApiValidationState.Succeeded;
                return true;
            }


            return false;
        }
    }
}
