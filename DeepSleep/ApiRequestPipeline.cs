namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestPipeline : IApiRequestPipeline
    {
        private Dictionary<int, ApiRequestDelegateHandler> _pipeline;

        /// <summary>Gets the registered pipeline.</summary>
        /// <value>The registered pipeline.</value>
        Dictionary<int, ApiRequestDelegateHandler> IApiRequestPipeline.RegisteredPipeline
        {
            get
            {
                return _pipeline;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestPipeline"/> class.
        /// </summary>
        public ApiRequestPipeline()
        {
            _pipeline = new Dictionary<int, ApiRequestDelegateHandler>();
        }

        /// <summary>Uses the pipeline component.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual IApiRequestPipeline UsePipelineComponent<T>()
        {
            var type = typeof(T);

            var methods = type.GetMethods()
                .Where(m => m.Name == "Invoke")
                .Where(m => m.ReturnType == typeof(Task));

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                if (parameters != null && parameters.Length > 0)
                {
                    if (parameters[0].ParameterType == typeof(IApiRequestContextResolver))
                    {
                        var delegateHandler = new ApiRequestDelegateHandler(this, _pipeline.Count, type, method);
                        _pipeline.Add(_pipeline.Count, delegateHandler);
                        break;
                    }
                }
            }

            return this;
        }

        /// <summary>Runs this instance.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public virtual async Task Run(IApiRequestContextResolver contextResolver)
        {
            var first = _pipeline.Count > 0
                ? _pipeline[0]
                : null;

            if (first != null)
            {
                await first.TaskInvoker(contextResolver).ConfigureAwait(false);
            }
        }
    }
}
