using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace DeepSleep
{
    /// <summary>The API response info.
    /// </summary>
    public class ApiResponseInfo
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseInfo"/> class.
        /// </summary>
        public ApiResponseInfo()
        {
            Headers = new List<ApiHeader>();
        }

        #endregion

        /// <summary>Gets or sets the response object.</summary>
        /// <value>The response object.</value>
        public ApiResponse ResponseObject { get; set; }

        /// <summary>Gets or sets the raw response object.</summary>
        /// <value>The raw response object.</value>
        public byte[] RawResponseObject { get; set; }

        /// <summary>Gets or sets the content language.</summary>
        /// <value>The content language.</value>
        public string ContentLanguage { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public MediaHeaderValueWithQualityString ContentType { get; set; }

        /// <summary>Gets or sets the length of the content.</summary>
        /// <value>The length of the content.</value>
        public long ContentLength { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public List<ApiHeader> Headers { get; set; }
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
            return (response?.ResponseObject?.StatusCode).IsBetween(200, 299);
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
    }
}