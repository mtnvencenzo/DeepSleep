namespace Api.DeepSleep.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class CommonErrorResponse
    {
        public List<ErrorMessage> Messages { get; set; } = new List<ErrorMessage>();
    }
}
