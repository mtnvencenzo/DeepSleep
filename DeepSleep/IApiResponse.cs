namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiResponse
    {
        /// <summary>Gets or sets the response.</summary>
        /// <value>The response.</value>
        object Response { get; set; }

        /// <summary>Gets or sets the status code.</summary>
        /// <value>The status code.</value>
        int StatusCode { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        IList<ApiHeader> Headers { get; set; }
    }
}
