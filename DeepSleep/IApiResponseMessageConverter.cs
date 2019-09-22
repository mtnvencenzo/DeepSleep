using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiResponseMessageConverter
    {
        /// <summary>Converts the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        ApiResponseMessage Convert(string message);
    }
}
