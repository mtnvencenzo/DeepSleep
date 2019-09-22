using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatus"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="status">The status.</param>
        public HttpStatus(int code, string status)
        {
            Code = code;
            Status = status;
        }

        /// <summary>Gets the code.</summary>
        /// <value>The code.</value>
        public int Code { get; private set; }

        /// <summary>Gets the status.</summary>
        /// <value>The status.</value>
        public string Status { get; private set; }

        /// <summary>Gets the ok.</summary>
        /// <value>The ok.</value>
        public static HttpStatus OK
        {
            get
            {
                return new HttpStatus(200, "OK");
            }
        }

        /// <summary>Gets the created.</summary>
        /// <value>The created.</value>
        public static HttpStatus Created
        {
            get
            {
                return new HttpStatus(201, "Created");
            }
        }

        /// <summary>Gets the accepted.</summary>
        /// <value>The accepted.</value>
        public static HttpStatus Accepted
        {
            get
            {
                return new HttpStatus(202, "Accepted");
            }
        }

        /// <summary>Gets the bad request.</summary>
        /// <value>The bad request.</value>
        public static HttpStatus BadRequest
        {
            get
            {
                return new HttpStatus(400, "Bad Request");
            }
        }

        /// <summary>Gets the not found.</summary>
        /// <value>The not found.</value>
        public static HttpStatus NotFound
        {
            get
            {
                return new HttpStatus(404, "Not Found");
            }
        }

        /// <summary>Gets the un authorized.</summary>
        /// <value>The un authorized.</value>
        public static HttpStatus Unauthorized
        {
            get
            {
                return new HttpStatus(401, "Unauthorized");
            }
        }

        /// <summary>Gets the forbidden.</summary>
        /// <value>The forbidden.</value>
        public static HttpStatus Forbidden
        {
            get
            {
                return new HttpStatus(403, "Forbidden");
            }
        }

        /// <summary>Gets the internal server error.</summary>
        /// <value>The internal server error.</value>
        public static HttpStatus InternalServerError
        {
            get
            {
                return new HttpStatus(500, "Internal Server Error");
            }
        }
    }
}
