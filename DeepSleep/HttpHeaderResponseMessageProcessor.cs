using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiResponseMessageProcessor" />
    public class HttpHeaderResponseMessageProcessor : IApiResponseMessageProcessor
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Process(ApiRequestContext context)
        {
            if (context.ProcessingInfo?.ExtendedMessages != null)
            {
                context.ProcessingInfo.ExtendedMessages.ForEach(m => context.ResponseInfo.AddHeader("X-Api-Message", m.FormatForHeader()));
            }

            var source = new TaskCompletionSource<object>();
            source.SetResult(null);
            return source.Task;
        }
    }
}
