namespace DeepSleep.Pipeline
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public abstract class PipelineComponentBase : IPipelineComponent
    {
        /// <summary>The apinext</summary>
        protected readonly ApiRequestDelegate apinext;

        /// <summary>Initializes a new instance of the <see cref="PipelineComponentBase"/> class.</summary>
        /// <param name="next">The next.</param>
        public PipelineComponentBase(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public abstract Task Invoke(IApiRequestContextResolver contextResolver);
    }
}
