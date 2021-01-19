namespace Api.DeepSleep.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class CommonErrorResponse
    {
        /// <summary>Gets or sets the messages.</summary>
        /// <value>The messages.</value>
        public List<ErrorMessage> Messages { get; set; } = new List<ErrorMessage>();
    }
}
