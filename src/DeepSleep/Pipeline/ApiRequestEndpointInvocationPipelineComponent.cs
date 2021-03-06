﻿namespace DeepSleep.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestEndpointInvocationPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestEndpointInvocationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestEndpointInvocationPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var beforePipelines = context?.Configuration?.PipelineComponents?
                .Where(p => p.Placement == PipelinePlacement.BeforeEndpointInvocation)
                .OrderBy(p => p.Order)
                .ToList() ?? new List<IRequestPipelineComponent>();

            var afterPipelines = context?.Configuration?.PipelineComponents?
                .Where(p => p.Placement == PipelinePlacement.AfterEndpointInvocation)
                .OrderBy(p => p.Order)
                .ToList() ?? new List<IRequestPipelineComponent>();

            foreach (var pipeline in beforePipelines)
            {
                await pipeline.Invoke(contextResolver).ConfigureAwait(false);
            }

            if (await context.ProcessHttpEndpointInvocation().ConfigureAwait(false))
            {
                foreach (var pipeline in afterPipelines)
                {
                    await pipeline.Invoke(contextResolver).ConfigureAwait(false);
                }

                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            else
            {
                foreach (var pipeline in afterPipelines)
                {
                    await pipeline.Invoke(contextResolver).ConfigureAwait(false);
                }
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
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpEndpointInvocation(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Routing?.Route?.Location?.MethodInfo != null)
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
                    foreach (var param in context.Routing.Route.Location.MethodInfo.GetParameters())
                    {
                        if (!addedUriParam && context.Request.InvocationContext.UriModel != null && param.GetCustomAttribute<InUriAttribute>() != null)
                        {
                            parameters.Add(context.Request.InvocationContext.UriModel);
                            addedUriParam = true;
                        }
                        else if (!addedBodyParam && context.Request.InvocationContext.BodyModel != null && param.GetCustomAttribute<InBodyAttribute>() != null)
                        {
                            parameters.Add(context.Request.InvocationContext.BodyModel);
                            addedBodyParam = true;
                        }
                        else
                        {
                            var simpleParameter = context.Request.InvocationContext.SimpleParameters
                                .Where(p => p.Key.Name == param.Name && p.Key.ParameterType == param.ParameterType)
                                .FirstOrDefault();

                            if (simpleParameter.Value != null)
                            {
                                parameters.Add(simpleParameter.Value);
                            }
                            else
                            {
                                parameters.Add(param.ParameterType.GetDefaultValue());
                            }
                        }
                    }

                    // -----------------------------------------------------
                    // Invoke the controller method with the parameters list
                    // -----------------------------------------------------

                    object endpointResponse;

                    try
                    {
                        endpointResponse = context.Routing.Route.Location.MethodInfo.Invoke(
                            context.Request.InvocationContext.ControllerInstance,
                            parameters.ToArray());
                    }
                    catch (TargetInvocationException ex)
                    {
                        if (ex.InnerException != null)
                        {
                            throw ex.InnerException;
                        }
                        else
                        {
                            throw;
                        }
                    }

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

                    if (endpointResponse as IApiResponse != null)
                    {
                        context.Response.ResponseObject = ((IApiResponse)endpointResponse).Response;
                        context.Response.StatusCode = ((IApiResponse)endpointResponse).StatusCode;
                        context.Runtime.Internals.IsOverridingStatusCode = true;

                        var headers = ((IApiResponse)endpointResponse).Headers;

                        if (headers != null)
                        {
                            headers
                                .Where(h => h != null)
                                .ToList()
                                .ForEach(h => context.Response.AddHeader(h.Name, h.Value));
                        }

                        if (endpointResponse as IApiErrorResponse != null)
                        {
                            var errors = ((IApiErrorResponse)endpointResponse).Errors;

                            if (errors != null)
                            {
                                errors.ForEach(e => context.AddValidationError(e));
                            }
                        }
                    }
                    else
                    {
                        context.Response.ResponseObject = endpointResponse;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
