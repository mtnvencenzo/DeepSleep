namespace DeepSleep
{
    using DeepSleep.Pipeline;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestDelegateHandler
    {
        private readonly ApiRequestDelegate requestDelegate;
        private readonly IApiRequestPipeline pipeline;
        private readonly Type type;

        /// <summary>Initializes a new instance of the <see cref="ApiRequestDelegateHandler"/> class.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <param name="type">The type.</param>
        public ApiRequestDelegateHandler(IApiRequestPipeline pipeline, Type type)
        {
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

            var instance = Activator.CreateInstance(this.type, new object[] { next }) as IPipelineComponent;

            await instance.Invoke(contextResolver).ConfigureAwait(false);
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
