namespace DeepSleep.Pipeline
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseMessagePipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseMessagePipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseMessagePipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="responseMessageProcessorProvider">The response message processor provider.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiResponseMessageProcessorProvider responseMessageProcessorProvider)
        {
            await apinext.Invoke(contextResolver).ConfigureAwait(false);

            var context = contextResolver.GetContext();

            await context.ProcessHttpResponseMessages(responseMessageProcessorProvider).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseMessagePipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response messages.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseMessages(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseMessagePipelineComponent>();
        }

        /// <summary>Processes the HTTP response messages.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageProcessorProvider">The response message processor provider.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpResponseMessages(this ApiRequestContext context, IApiResponseMessageProcessorProvider responseMessageProcessorProvider)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if ((context.ProcessingInfo?.ExtendedMessages?.Count ?? 0) > 0)
                {
                    context.ProcessingInfo.ExtendedMessages.ClearDuplicates();
                    context.ProcessingInfo.ExtendedMessages.SortMessagesByCode();

                    if (responseMessageProcessorProvider != null)
                    {
                        foreach (var processor in responseMessageProcessorProvider.GetProcessors())
                        {
                            await processor.Process(context).ConfigureAwait(false);
                        }
                    }
                }
            }

            return true;
        }
    }
}
