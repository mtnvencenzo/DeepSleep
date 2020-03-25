namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResult
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="ApiResult"/> class.
        /// </summary>
        public ApiResult()
        {
            Messages = new List<ApiResponseMessage>();
        }

        #endregion

        /// <summary>
        /// The http status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>Gets or sets the messages.</summary>
        /// <value>The messages.</value>
        public List<ApiResponseMessage> Messages { get; set; }
    }
}
