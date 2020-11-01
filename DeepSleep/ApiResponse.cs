namespace DeepSleep
{
    using System;
    using System.Collections.Generic;

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
        public IList<ApiHeader> Headers { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseExtensions
    {
        /// <summary>
        /// Optionally adds the entity tag (ETag) and last modifed date (Last-Modified) to the response headers.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="etag"></param>
        /// <param name="lastModified"></param>
        /// <returns></returns>
        public static ApiResponse AddEntityCaching(this ApiResponse response, string etag = null, DateTimeOffset? lastModified = null)
        {
            if (!string.IsNullOrWhiteSpace(etag))
            {
                if (response.Headers.HasHeader("ETag"))
                {
                    response.Headers.SetValue("ETag", etag);
                }
                else
                {
                    response.Headers.Add(new ApiHeader
                    {
                        Name = "ETag",
                        Value = etag
                    });
                }
            }

            if (lastModified != null)
            {
                if (response.Headers.HasHeader("Last-Modified"))
                {
                    response.Headers.SetValue("Last-Modified", lastModified.Value.ToString("r"));
                }
                else
                {
                    response.Headers.Add(new ApiHeader
                    {
                        Name = "Last-Modified",
                        Value = lastModified.Value.ToString("r")
                    });
                }
            }

            return response;
        }
    }
}
