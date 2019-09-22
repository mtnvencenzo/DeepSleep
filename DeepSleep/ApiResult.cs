using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
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

        /// <summary>Gets or sets the messages.</summary>
        /// <value>The messages.</value>
        public List<ApiResponseMessage> Messages { get; set; }
    }
}
