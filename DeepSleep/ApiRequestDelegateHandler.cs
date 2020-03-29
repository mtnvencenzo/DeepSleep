namespace DeepSleep
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestDelegateHandler
    {
        private readonly MethodInfo target;
        private readonly ApiRequestDelegate requestDelegate;
        private readonly IApiRequestPipeline pipeline;
        private readonly Type type;
        private ParameterInfo[] parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestDelegateHandler" /> class.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        public ApiRequestDelegateHandler(IApiRequestPipeline pipeline, Type type, MethodInfo target)
        {
            this.target = target;
            this.pipeline = pipeline;
            this.requestDelegate = new ApiRequestDelegate(TaskInvoker);
            this.type = type;
        }
        
        /// <summary>Tasks the invoker.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task TaskInvoker(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();
            var registeredPipeline = this.pipeline.RegisteredPipeline;
            var index = registeredPipeline.FirstOrDefault(p => p.Value.type == this.type).Key;

            var next = (index >= registeredPipeline.Count - 1)
                ? new ApiRequestDelegate(TaskFinisher)
                : registeredPipeline[index + 1].requestDelegate;

            var instance = Activator.CreateInstance(this.type, new object[] { next });
            var targetInvocationParameters = new List<object>();

            if (this.parameters == null)
            {
                this.parameters = this.target.GetParameters();
            }

            foreach (var methodParam in this.parameters)
            {
                if (methodParam.ParameterType == typeof(IApiRequestContextResolver))
                {
                    targetInvocationParameters.Add(contextResolver);
                }
                else
                {
                    if (context?.RequestServices != null)
                    {
                        try
                        {
                            var service = context.RequestServices.GetService(methodParam.ParameterType);
                            targetInvocationParameters.Add(service);
                        }
                        catch (System.Exception)
                        {
                            targetInvocationParameters.Add(null);
                        }
                    }
                    else
                    {
                        targetInvocationParameters.Add(null);
                    }
                }
            }

            var task = (Task)this.target.Invoke(instance, targetInvocationParameters.ToArray());
            await task.ConfigureAwait(false);
        }

        /// <summary>Tasks the finisher.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public static Task TaskFinisher(IApiRequestContextResolver contextResolver)
        {
            return Task.CompletedTask;
        }
    }
}
