namespace DeepSleep.Pipeline
{
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestEndpointInvocationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestEndpointInvocationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestEndpointInvocationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, ILogger<ApiRequestEndpointInvocationPipelineComponent> logger)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpEndpointInvocation(logger).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
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

        /// <summary>Processes the HTTP endpoint invocation.</summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpEndpointInvocation(this ApiRequestContext context, ILogger logger)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RequestInfo?.InvocationContext?.ControllerMethod != null)
                {
                    var parameters = new List<object>();
                    bool addedUriParam = false;
                    bool addedBodyParam = false;

                    // -----------------------------------------------------------
                    // Build the parameters list to invoke the controller method
                    // This includes the UriModel and BodyModel if they exist.
                    // If any other parameters exists on the controller method
                    // they'll be passed a null value.  A possible enhancement
                    // would be to pull the extra parameters from the DI container
                    // -----------------------------------------------------------
                    foreach (var param in context.RequestInfo.InvocationContext.ControllerMethod.GetParameters())
                    {
                        if (!addedUriParam && context.RequestInfo.InvocationContext.UriModel != null && param.GetCustomAttribute<UriBoundAttribute>() != null)
                        {
                            parameters.Add(context.RequestInfo.InvocationContext.UriModel);
                            addedUriParam = true;
                        }
                        else if (!addedBodyParam && context.RequestInfo.InvocationContext.BodyModel != null && param.GetCustomAttribute<BodyBoundAttribute>() != null)
                        {
                            parameters.Add(context.RequestInfo.InvocationContext.BodyModel);
                            addedBodyParam = true;
                        }
                        else
                        {
                            parameters.Add(param.ParameterType.GetDefaultValue());
                        }
                    }

                    // -----------------------------------------------------
                    // Invoke the controller method with the parameters list
                    // -----------------------------------------------------
                    logger?.LogInformation($"Invoking controller method {context.RequestInfo.InvocationContext.Controller.GetType().FullName}::{context.RequestInfo.InvocationContext.ControllerMethod.Name}");

                    var endpointResponse = context.RequestInfo.InvocationContext.ControllerMethod.Invoke(
                        context.RequestInfo.InvocationContext.Controller,
                        parameters.ToArray());

                    // -----------------------------------------------------
                    // If the response is awaitable then handle
                    // the await on the result
                    // -----------------------------------------------------
                    if (endpointResponse as Task != null)
                    {
                        await ((Task)endpointResponse).ConfigureAwait(false);
                        var resultProperty = endpointResponse.GetType().GetProperty("Result");

                        var response = resultProperty?.GetValue(endpointResponse);

                        if (response != null && response.GetType().FullName != "System.Threading.Tasks.VoidTaskResult")
                        {
                            endpointResponse = response;
                        }
                        else
                        {
                            endpointResponse = null;
                        }
                    }

                    context.ResponseInfo.ResponseObject = endpointResponse;
                }

                return true;
            }

            return false;
        }
    }
}
