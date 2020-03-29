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
        private readonly IList<ApiRequestDelegateHandler> pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestPipeline"/> class.
        /// </summary>
        public ApiRequestPipeline()
        {
            this.pipeline = new List<ApiRequestDelegateHandler>();
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
                        var delegateHandler = new ApiRequestDelegateHandler(this, type, method);
                        pipeline.Add(delegateHandler);
                        break;
                    }
                }
            }

            return this;
        }

        /// <summary>Uses the pipeline component.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index position to insert the component into the pipeline.</param>
        /// <returns></returns>
        public virtual IApiRequestPipeline UsePipelineComponent<T>(int index)
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
                        var delegateHandler = new ApiRequestDelegateHandler(this, type, method);

                        if (index > -1)
                        {
                            if (index >= this.pipeline.Count)
                            {
                                pipeline.Add(delegateHandler);
                            }
                            else
                            {
                                pipeline.Insert(index, delegateHandler);
                            }
                        }
                        else
                        {
                            pipeline.Add(delegateHandler);
                        }
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
            var first = pipeline.FirstOrDefault();

            if (first != null)
            {
                await first.TaskInvoker(contextResolver).ConfigureAwait(false);
            }
        }

        /// <summary>Gets the registered pipeline.</summary>
        /// <value>The registered pipeline.</value>
        Dictionary<int, ApiRequestDelegateHandler> IApiRequestPipeline.RegisteredPipeline
        {
            get
            {
                return pipeline.ToDictionary(p => this.pipeline.IndexOf(p), p => p);
            }
        }

    }
}
