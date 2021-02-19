namespace DeepSleep.Tests
{
    using DeepSleep;
    using DeepSleep.Pipeline;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            return Task.CompletedTask;
        }
    }
}
