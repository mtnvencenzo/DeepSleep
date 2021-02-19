namespace DeepSleep.Pipeline
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class RequestPipelineComponent<T> : IRequestPipelineComponent where T : IPipelineComponent
    {
        /// <summary>Initializes a new instance of the <see cref="RequestPipelineComponent{T}"/> class.</summary>
        /// <param name="placement">The placement.</param>
        /// <param name="order">The order.</param>
        public RequestPipelineComponent(PipelinePlacement placement, int order = 0)
        {
            this.Placement = placement;

            this.Order = order < 0
                ? 0
                : order;
        }

        /// <summary>Gets the placement.</summary>
        /// <value>The placement.</value>
        public PipelinePlacement Placement { get; }

        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        public int Order { get; }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                IPipelineComponent pipeline = null;

                try
                {
                    if (context.RequestServices != null)
                    {
                        pipeline = context.RequestServices.GetService<T>();
                    }
                }
                catch { }

                if (pipeline == null)
                {
                    try
                    {
                        pipeline = Activator.CreateInstance<T>();
                    }
                    catch { }
                }

                if (pipeline != null)
                {
                    await pipeline.Invoke(contextResolver).ConfigureAwait(false);
                }
            }
        }
    }
}
