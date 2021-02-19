namespace DeepSleep.Pipeline
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="DeepSleep.Pipeline.IRequestPipelineComponent" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiRequestPipelineAttribute : Attribute, IRequestPipelineComponent
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRequestPipelineAttribute"/> class.</summary>
        /// <param name="pipelineType">Type of the pipeline.</param>
        /// <param name="placement">The placement.</param>
        /// <param name="order">The order.</param>
        public ApiRequestPipelineAttribute(Type pipelineType, PipelinePlacement placement, int order = 0)
        {
            if (pipelineType == null)
            {
                throw new ArgumentNullException(nameof(pipelineType));
            }

            if (pipelineType.GetInterface(nameof(IPipelineComponent), false) == null)
            {
                throw new ArgumentException($"{nameof(pipelineType)} must implement interface {typeof(IPipelineComponent).AssemblyQualifiedName}");
            }

            this.PipelineType = pipelineType;
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

        /// <summary>Gets the type of the pipeline.</summary>
        /// <value>The type of the pipeline.</value>
        public Type PipelineType { get; }

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
                        pipeline = context.RequestServices.GetService(Type.GetType(PipelineType.AssemblyQualifiedName)) as IPipelineComponent;
                    }
                }
                catch { }

                if (pipeline == null)
                {
                    try
                    {
                        pipeline = Activator.CreateInstance(Type.GetType(PipelineType.AssemblyQualifiedName)) as IPipelineComponent;
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
