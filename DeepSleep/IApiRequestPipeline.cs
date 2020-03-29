namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRequestPipeline
    {
        /// <summary>Uses the pipeline component.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IApiRequestPipeline UsePipelineComponent<T>();

        /// <summary>Uses the pipeline component.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index">The index position to insert the component into the pipeline.</param>
        /// <returns></returns>
        IApiRequestPipeline UsePipelineComponent<T>(int index);

        /// <summary>Gets the registered pipeline.</summary>
        /// <value>The registered pipeline.</value>
        Dictionary<int, ApiRequestDelegateHandler> RegisteredPipeline { get; }

        /// <summary>Runs this instance.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        Task Run(IApiRequestContextResolver contextResolver);
    }
}
