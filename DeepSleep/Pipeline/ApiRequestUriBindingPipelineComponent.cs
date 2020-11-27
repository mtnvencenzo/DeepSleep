namespace DeepSleep.Pipeline
{
    using DeepSleep.Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestUriBindingPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestUriBindingPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestUriBindingPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form url serializer.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestUriBinding(serviceProvider, responseMessageConverter, formUrlEncodedObjectSerializer).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestUriBindingPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request URI binding.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestUriBinding(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestUriBindingPipelineComponent>();
        }



        /// <summary>Processes the HTTP request URI binding.</summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <param name="formUrlEncodedObjectSerializer">The form url serializer.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<bool> ProcessHttpRequestUriBinding(this ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter, IFormUrlEncodedObjectSerializer formUrlEncodedObjectSerializer)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                var addedBindingError = false;

                if (context.RequestInfo.InvocationContext?.UriModelType != null)
                {
  
                    var nameValues = new Dictionary<string, string>();

                    if ((context.RouteInfo?.RoutingItem?.RouteVariables?.Count ?? 0) > 0)
                    {
                        foreach (var routeVar in context.RouteInfo.RoutingItem.RouteVariables.Keys)
                        {
                            if (!nameValues.TryAdd(routeVar, context.RouteInfo.RoutingItem.RouteVariables[routeVar]))
                            {
                                addedBindingError = true;
                                context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.UriRouteBindingError,
                                    routeVar)));
                            }
                        }
                    }

                    if ((context.RequestInfo.QueryVariables?.Count ?? 0) > 0)
                    {
                        foreach (var qvar in context.RequestInfo.QueryVariables.Keys)
                        {
                            nameValues.TryAdd(qvar, context.RequestInfo.QueryVariables[qvar]);
                        }
                    }

                    if (!addedBindingError)
                    {
                        var bindingValues = nameValues
                            .Select(kv => $"{kv.Key}={kv.Value}");

                        var formUrlEncoded = string.Join('&', bindingValues);

                        try
                        {
                            var uriModel = await formUrlEncodedObjectSerializer.Deserialize(
                                formUrlEncoded, 
                                context.RequestInfo.InvocationContext.UriModelType).ConfigureAwait(false);

                            context.RequestInfo.InvocationContext.UriModel = uriModel;
                            return true;
                        }
                        catch (JsonException ex)
                        {
                            addedBindingError = true;
                            context.ProcessingInfo.ExtendedMessages.Add(responseMessageConverter.Convert(string.Format(ValidationErrors.UriBindingError,
                                ex.Path.TrimStart('$', '.'))));
                        }
                    }
                }

                if (addedBindingError)
                {
                    context.ResponseInfo.ResponseObject = new ApiResponse
                    {
                        StatusCode = 400
                    };
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
