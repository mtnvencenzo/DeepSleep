namespace DeepSleep
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiResponseMessageProcessor" />
    public class ApiResultResponseMessageProcessor : IApiResponseMessageProcessor
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Process(ApiRequestContext context)
        {
            if (context?.ResponseInfo != null && context.ResponseInfo.HasSuccessStatus() == false)
            {
                if (context.ProcessingInfo?.ExtendedMessages != null && context.ProcessingInfo.ExtendedMessages.Count > 0)
                {
                    if (context.ResponseInfo.ResponseObject == null)
                    {
                        context.ResponseInfo.ResponseObject = new ApiResult
                        {
                            StatusCode = context.ResponseInfo.StatusCode,
                            Messages = context.ProcessingInfo.ExtendedMessages
                        };
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
