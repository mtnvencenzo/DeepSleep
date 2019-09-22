using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestDelegateHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestDelegateHandler" /> class.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="index">The index.</param>
        /// <param name="type">The type.</param>
        /// <param name="target">The target.</param>
        public ApiRequestDelegateHandler(IApiRequestPipeline pipeline, int index, Type type, MethodInfo target)
        {
            _target = target;
            _pipeline = pipeline;
            _index = index;
            _delegate = new ApiRequestDelegate(TaskInvoker);
            _type = type;
        }
        
        private MethodInfo _target;
        private ApiRequestDelegate _delegate;
        private IApiRequestPipeline _pipeline;
        private readonly int _index;
        private Type _type;


        /// <summary>Tasks the invoker.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task TaskInvoker(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            var next = (_index >= _pipeline.RegisteredPipeline.Count - 1)
                ? new ApiRequestDelegate(TaskFinisher)
                : _pipeline.RegisteredPipeline[_index + 1]._delegate;

            var constructor = _type.GetConstructor(new Type[] { typeof(ApiRequestDelegate) });

            var instance = Activator.CreateInstance(_type, new object[] { next });

            var targetInvocationParameters = new List<object>();
            var methodParams = _target.GetParameters();

            foreach (var methodParam in methodParams)
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

            var task = (Task)_target.Invoke(instance, targetInvocationParameters.ToArray());
            await task;
        }

        /// <summary>Tasks the finisher.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public static Task TaskFinisher(IApiRequestContextResolver contextResolver)
        {
            var source = new TaskCompletionSource<object>();
            source.SetResult(null);
            return source.Task;
        }
    }
}
