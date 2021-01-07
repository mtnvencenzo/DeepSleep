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
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestInvocationInitializerPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestInvocationInitializerPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            if (await context.ProcessHttpEndpointInitialization().ConfigureAwait(false))
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
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Routing item's controller type is null
        /// or
        /// Routing item's endpoint name is null
        /// or
        /// </exception>
        internal static Task<bool> ProcessHttpEndpointInitialization(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (context.Routing?.Route?.Location?.Controller == null)
                {
                    throw new Exception("Routing item's controller type is null");
                }

                if (string.IsNullOrWhiteSpace(context.Routing.Route.Location.Endpoint))
                {
                    throw new Exception("Routing item's endpoint name is null");
                }


                object endpointController = null;

                if (context.RequestServices != null)
                {
                    try
                    {
                        endpointController = context.RequestServices.GetService(context.Routing.Route.Location.Controller);
                    }
                    catch { }
                }

                if (endpointController == null)
                {
                    try
                    {
                        var constructors = context.Routing.Route.Location.Controller.GetConstructors();

                        if (constructors.Length == 0 || constructors.FirstOrDefault(c => c.GetParameters().Length == 0) != null)
                        {
                            endpointController = Activator.CreateInstance(context.Routing.Route.Location.Controller);
                        }

                        var firstConstructor = constructors.First();
                        var constructorParameters = new List<object>();

                        firstConstructor.GetParameters().ToList().ForEach(p => constructorParameters.Add(null));
                        endpointController = Activator.CreateInstance(context.Routing.Route.Location.Controller, constructorParameters.ToArray());
                    }
                    catch (Exception)
                    {
                        throw new Exception(string.Format("Endpoint controller could not be instantiated.  Ensure that the controller has a parameterless contructor or is registered in the DI container"));
                    }
                }

                var simpleParameters = new Dictionary<ParameterInfo, object>();

                if (context.Routing.Route.Location.SimpleParameters != null)
                {
                    simpleParameters = context.Routing.Route.Location.SimpleParameters
                        .ToDictionary((k) => k, (k) => null as object);
                }

                context.Request.InvocationContext = new ApiInvocationContext
                {
                    ControllerInstance = endpointController,
                    SimpleParameters = simpleParameters
                };

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
