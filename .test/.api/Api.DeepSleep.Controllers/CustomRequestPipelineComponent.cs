namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Pipeline;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Pipeline.IPipelineComponent" />
    public class CustomRequestPipelineComponent : IPipelineComponent
    {
        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();

            if (context != null)
            {
                context.TryAddItem(nameof(CustomRequestPipelineComponent), "SET");
            }

            return Task.CompletedTask;
        }
    }
}
