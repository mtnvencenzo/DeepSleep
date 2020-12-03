namespace DeepSleep.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestInvocationInitializerPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestInvocationInitializerPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestInvocationInitializerPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpEndpointInitialization(context.RequestServices).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestInvocationInitializerPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request invocation initializer.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestInvocationInitializer(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestInvocationInitializerPipelineComponent>();
        }

        /// <summary>Processes the HTTP endpoint initialization.</summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Routing item's controller type is null
        /// or
        /// Routing item's endpoint name is null
        /// or
        /// </exception>
        internal static Task<bool> ProcessHttpEndpointInitialization(this ApiRequestContext context, IServiceResolver serviceProvider)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.RouteInfo?.RoutingItem?.EndpointLocation?.Controller == null)
                {
                    throw new Exception("Routing item's controller type is null");
                }

                if (string.IsNullOrWhiteSpace(context.RouteInfo.RoutingItem.EndpointLocation.Endpoint))
                {
                    throw new Exception("Routing item's endpoint name is null");
                }

                MethodInfo method = context.RouteInfo.RoutingItem.EndpointLocation.GetEndpointMethod();
                object endpointController = null;


                if (serviceProvider != null)
                {
                    try
                    {
                        endpointController = serviceProvider.GetService(context.RouteInfo.RoutingItem.EndpointLocation.Controller);
                    }
                    catch
                    {
                    }
                }


                if (endpointController == null)
                {
                    try
                    {
                        var constructors = context.RouteInfo.RoutingItem.EndpointLocation.Controller.GetConstructors();

                        if (constructors.Length == 0 || constructors.FirstOrDefault(c => c.GetParameters().Length == 0) != null)
                        {
                            endpointController = Activator.CreateInstance(context.RouteInfo.RoutingItem.EndpointLocation.Controller);
                        }

                        var firstConstructor = constructors.First();
                        var constructorParameters = new List<object>();

                        firstConstructor.GetParameters().ToList().ForEach(p => constructorParameters.Add(null));
                        endpointController = Activator.CreateInstance(context.RouteInfo.RoutingItem.EndpointLocation.Controller, constructorParameters.ToArray());
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Endpoint controller could not be instantiated.  Ensure that the controller has a parameterless contructor or is registered in the DI container"));
                    }
                }

                var uriParameter = context.RouteInfo.RoutingItem.EndpointLocation.GetUriParameter();
                var bodyParameter = context.RouteInfo.RoutingItem.EndpointLocation.GetBodyParameter();
                var simpleParameters = context.RouteInfo.RoutingItem.EndpointLocation.GetSimpleParameters();

                context.RequestInfo.InvocationContext = new ApiInvocationContext
                {
                    Controller = endpointController,
                    ControllerMethod = method,
                    UriModelType = uriParameter?.ParameterType,
                    BodyModelType = bodyParameter?.ParameterType,
                    SimpleParameters = simpleParameters
                };

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
