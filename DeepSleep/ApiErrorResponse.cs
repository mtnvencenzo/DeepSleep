namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.ApiResponse" />
    /// <seealso cref="DeepSleep.IApiErrorResponse" />
    public class ApiErrorResponse : ApiResponse, IApiErrorResponse
    {
        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        public virtual IList<string> Errors { get; set; }
    }
}
