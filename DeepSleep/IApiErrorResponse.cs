namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiResponse" />
    internal interface IApiErrorResponse : IApiResponse
    {
        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        IList<string> Errors { get; set; }
    }
}
