namespace DeepSleep.Pipeline
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        Task Invoke(IApiRequestContextResolver contextResolver);
    }
}
