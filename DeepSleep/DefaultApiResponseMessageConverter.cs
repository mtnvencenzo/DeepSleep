namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiResponseMessageConverter" />
    public class DefaultApiResponseMessageConverter : IApiResponseMessageConverter
    {
        /// <summary>Converts the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ApiResponseMessage Convert(string message)
        {
            return (!string.IsNullOrWhiteSpace(message))
                ? ApiResponseMessage.BuildResponseMessageFromResource(message)
                : null;
        }
    }
}
