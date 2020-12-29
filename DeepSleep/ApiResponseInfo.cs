namespace DeepSleep
{
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    /// <summary>The API response info.
    /// </summary>
    [DebuggerDisplay("{StatusCode}")]
    public class ApiResponseInfo
    {
        /// <summary>Gets or sets the status code.</summary>
        /// <value>The status code.</value>
        public virtual int StatusCode { get; set; } = 200;

        /// <summary>Gets or sets the response object.</summary>
        /// <value>The response object.</value>
        public virtual object ResponseObject { get; set; }

        /// <summary>Gets or sets the content language.</summary>
        /// <value>The content language.</value>
        public virtual string ContentLanguage { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public virtual ContentTypeHeader ContentType { get; set; }

        /// <summary>Gets or sets the length of the content.</summary>
        /// <value>The length of the content.</value>
        public virtual long ContentLength { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public virtual IList<ApiHeader> Headers { get; set; } = new List<ApiHeader>();

        /// <summary>Gets or sets the cokkies associated with the response.</summary>
        /// <value>The cokies.</value>
        public virtual IList<ApiCookie> Cookies { get; set; } = new List<ApiCookie>();

        /// <summary>The response header date used in generating the response
        /// </summary>
        public virtual DateTimeOffset? Date { get; set; }

        /// <summary>The response writer available to write the response
        /// </summary>
        public virtual IFormatStreamReaderWriter ResponseWriter { get; set; }

        /// <summary>The response writer formatting options available to write the response
        /// </summary>
        public virtual IFormatStreamOptions ResponseWriterOptions { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseInfoExtensionMethods
    {
        /// <summary>
        /// Determines whether [has success status].
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>
        ///   <c>true</c> if [has success status] [the specified response]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasSuccessStatus(this ApiResponseInfo response)
        {
            if (response == null)
                return false;

            return response.StatusCode.IsBetween(200, 299);
        }

        /// <summary>Gets the header values.</summary>
        /// <param name="response">The response.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IList<string> GetHeaderValues(this ApiResponseInfo response, string name)
        {
            if (response == null || string.IsNullOrWhiteSpace(name))
                return new string[] { };

            List<string> values = new List<string>();

            response.Headers.ForEach(h =>
            {
                if (string.Compare(h.Name, name, true) == 0)
                {
                    if (!values.Contains(h.Value))
                    {
                        values.Add(h.Value);
                    }
                }
            });

            return values;
        }

        /// <summary>
        /// Optionally adds the entity tag (ETag) and last modifed date (Last-Modified) to the response headers.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="etag"></param>
        /// <param name="lastModified"></param>
        /// <returns></returns>
        public static ApiResponseInfo AddEntityCaching(this ApiResponseInfo response, string etag = null, DateTimeOffset? lastModified = null)
        {
            if (response == null)
                return response;

            if (response.Headers == null)
            {
                response.Headers = new List<ApiHeader>();
            }

            if (!string.IsNullOrWhiteSpace(etag))
            {
                if (response.Headers.HasHeader("ETag"))
                {
                    response.Headers.SetValue("ETag", etag);
                }
                else
                {
                    response.Headers.Add(new ApiHeader("ETag", etag));
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
                    response.Headers.Add(new ApiHeader("Last-Modified", lastModified.Value.ToString("r")));
                }
            }

            return response;
        }

        /// <summary>Sets the HTTP status.</summary>
        /// <param name="response">The response.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static ApiResponseInfo SetHttpStatus(this ApiResponseInfo response, int status)
        {
            if (response == null)
            {
                response = new ApiResponseInfo();
            }

            response.StatusCode = status;
            return response;
        }

        /// <summary>Adds the header.</summary>
        /// <param name="response">The response.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ApiResponseInfo AddHeader(this ApiResponseInfo response, string name, string value)
        {
            if (response == null)
                return response;

            if (response.Headers == null)
            {
                response.Headers = new List<ApiHeader>();
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                response.Headers.Add(new ApiHeader(name, value));
            }

            return response;
        }

        /// <summary>Sets the HTTP header.</summary>
        /// <param name="response">The response.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ApiResponseInfo SetHttpHeader(this ApiResponseInfo response, string name, string value)
        {
            if (response == null)
            {
                response = new ApiResponseInfo();
            }

            if (response.Headers == null)
            {
                response.Headers = new List<ApiHeader>();
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                response.AddHeader(name, value);
            }

            return response;
        }
    }
}