using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiResponseMessageProcessor
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task Process(ApiRequestContext context); 
    }
}
