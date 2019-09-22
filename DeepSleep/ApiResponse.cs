using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiResponse
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        public ApiResponse()
        {
            StatusCode = 200;
            Headers = new List<ApiHeader>();
        }

        /// <summary>Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="body">The body.</param>
        public ApiResponse(int statusCode, object body)
        {
            StatusCode = statusCode;
            Body = body;
            Headers = new List<ApiHeader>();
        }

        /// <summary>Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        public ApiResponse(object body)
        {
            Body = body;
            Headers = new List<ApiHeader>();
        }

        #endregion

        /// <summary>Gets or sets the status code.</summary>
        /// <value>The status code.</value>
        public int StatusCode { get; set; }

        /// <summary>Gets or sets the body.</summary>
        /// <value>The body.</value>
        public object Body { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public List<ApiHeader> Headers { get; set; }
    }
}
