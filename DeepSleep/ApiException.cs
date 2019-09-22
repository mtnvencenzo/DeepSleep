// --------------------------------------------------------------------------------------------------------------------
// <copyright file="APIException.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Net;

namespace DeepSleep
{
    /// <summary></summary>
    public class ApiException : Exception
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="ApiException"/> class.</summary>
        /// <param name="statusCode">The status code.</param>
        public ApiException(HttpStatusCode statusCode)
            : this(null, statusCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code.</param>
        public ApiException(ApiResponseMessage message, HttpStatusCode statusCode)
            : this(message, statusCode, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <param name="innerException"></param>
        public ApiException(ApiResponseMessage message, HttpStatusCode statusCode, Exception innerException)
            : base(message != null ? message.Message : string.Empty, innerException)
        {
            ResponseMessage = message;
            StatusCode = statusCode;
        }

        #endregion

        /// <summary>Gets the response message.</summary>
        /// <value>The response message.</value>
        public ApiResponseMessage ResponseMessage { get; private set; }

        /// <summary>Gets the status code.</summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; private set; }
    }
}