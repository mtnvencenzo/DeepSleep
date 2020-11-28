﻿namespace DeepSleep
{
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>The API response info.
    /// </summary>
    public class ApiResponseInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseInfo"/> class.
        /// </summary>
        public ApiResponseInfo()
        {
            this.StatusCode = 200;
            this.Headers = new List<ApiHeader>();
            this.Cookies = new List<ApiCookie>();
        }

        /// <summary>Gets or sets the status code.</summary>
        /// <value>The status code.</value>
        public int StatusCode { get; set; }

        /// <summary>Gets or sets the response object.</summary>
        /// <value>The response object.</value>
        public virtual object ResponseObject { get; set; }

        /// <summary>Gets or sets the content language.</summary>
        /// <value>The content language.</value>
        public virtual string ContentLanguage { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public virtual MediaHeaderValueWithQualityString ContentType { get; set; }

        /// <summary>Gets or sets the length of the content.</summary>
        /// <value>The length of the content.</value>
        public virtual long ContentLength { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public virtual IList<ApiHeader> Headers { get; set; }

        /// <summary>Gets or sets the cokkies associated with the response.</summary>
        /// <value>The cokies.</value>
        public IList<ApiCookie> Cookies { get; set; }

        /// <summary>The response header date used in generating the response
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>The response writer available to write the response
        /// </summary>
        public virtual IFormatStreamReaderWriter ResponseWriter { get; set; }

        /// <summary>The response writer formatting options available to write the response
        /// </summary>
        public virtual IFormatStreamOptions ResponseWriterOptions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Dump()
        {
            var builder = new StringBuilder();

            if (this.Headers != null)
            {
                foreach (var header in this.Headers)
                {
                    builder.AppendLine($"{header.Name}: {header.Value}");
                }
            }

            return builder.ToString();
        }
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
            return (response?.StatusCode).IsBetween(200, 299);
        }

        /// <summary>Adds the header.</summary>
        /// <param name="response">The response.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ApiResponseInfo AddHeader(this ApiResponseInfo response, string name, string value)
        {
            response.Headers.Add(new ApiHeader { Name = name, Value = value });
            return response;
        }

        /// <summary>Gets the header values.</summary>
        /// <param name="response">The response.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string[] GetHeaderValues(this ApiResponseInfo response, string name)
        {
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

            return values.ToArray();
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