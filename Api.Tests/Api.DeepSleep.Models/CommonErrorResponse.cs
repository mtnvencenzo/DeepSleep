namespace Api.DeepSleep.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class CommonErrorResponse
    {
        public IList<ErrorMessage> Messages { get; set; } = new List<ErrorMessage>();
    }
}
