using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
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
                    if (context.ResponseInfo.ResponseObject?.Body == null)
                    {
                        context.ResponseInfo.ResponseObject.Body = new ApiResult
                        {
                            Messages = context.ProcessingInfo.ExtendedMessages
                        };
                    }
                }
            }



            var source = new TaskCompletionSource<object>();
            source.SetResult(null);
            return source.Task;
        }
    }
}
