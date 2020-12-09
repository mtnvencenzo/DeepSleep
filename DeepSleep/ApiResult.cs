namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResult
    {
        /// <summary>Gets or sets the messages.</summary>
        /// <value>The messages.</value>
        public List<ApiResultMessage> Messages { get; set; } = new List<ApiResultMessage>();
    }
}
