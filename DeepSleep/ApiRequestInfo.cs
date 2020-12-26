namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>The API request info.</summary>
    [DebuggerDisplay("[{Method?.ToUpper()}] {RequestUri}")]
    public class ApiRequestInfo
    {
        /// <summary>Gets or sets the request accept types.</summary>
        /// <value>The request accept types.</value>
        public virtual AcceptHeader Accept { get; set; }

        /// <summary>Gets or sets the accept language.</summary>
        /// <value>The accept language.</value>
        public virtual AcceptLanguageHeader AcceptLanguage { get; set; }

        /// <summary>Gets or sets the accept culture.</summary>
        /// <value>The accept culture.</value>
        public virtual CultureInfo AcceptCulture { get; set; }

        /// <summary>Gets or sets the accept charset.</summary>
        /// <value>The accept charset.</value>
        public virtual AcceptCharsetHeader AcceptCharset { get; set; }

        /// <summary>Gets or sets the accept encoding.</summary>
        /// <value>The accept encoding.</value>
        public virtual AcceptEncodingHeader AcceptEncoding { get; set; }

        /// <summary>Gets or sets the authentication info.</summary>
        /// <value>The authentication info.</value>
        public virtual ClientAuthentication ClientAuthenticationInfo { get; set; }

        /// <summary>Gets or sets the authorization info.</summary>
        /// <value>The authorization info.</value>
        public virtual ClientAuthorization ClientAuthorizationInfo { get; set; }

        /// <summary>Gets or sets the cross origin request.</summary>
        /// <value>The cross origin request.</value>
        public virtual CrossOriginRequestValues CrossOriginRequest { get; set; }

        /// <summary>Gets or sets the type of the content.</summary>
        /// <value>The type of the content.</value>
        public virtual ContentTypeHeader ContentType { get; set; }

        /// <summary>Gets or sets the If-Modified-Since conditional request header.</summary>
        /// <value>The value of the If-Modified-Since header..</value>
        public virtual DateTimeOffset? IfModifiedSince { get; set; }

        /// <summary>Gets or sets the If-Unmodified-Since conditional request header.</summary>
        /// <value>The value of the If-Unmodified-Since header..</value>
        public virtual DateTimeOffset? IfUnmodifiedSince { get; set; }

        /// <summary>Gets or sets the If-None-Match conditional request header.</summary>
        /// <value>The value of the If-None-Match header..</value>
        public virtual string IfNoneMatch { get; set; }

        /// <summary>Gets or sets the If-Match conditional request header.</summary>
        /// <value>The value of the If-Match header..</value>
        public virtual string IfMatch { get; set; }

        /// <summary>Gets or sets the cookies.</summary>
        /// <value>The cookies.</value>
        public Dictionary<string, string> Cookies { get; set; } = new Dictionary<string, string>();

        /// <summary>Gets or sets the length of the content.</summary>
        /// <value>The length of the content.</value>
        public virtual long? ContentLength { get; set; }

        /// <summary>Gets or sets the client specified correlation id for the request.</summary>
        /// <value>The correlation id.</value>
        public virtual string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pretty print].
        /// </summary>
        /// <value><c>true</c> if [pretty print]; otherwise, <c>false</c>.</value>
        public virtual bool PrettyPrint { get; set; }

        /// <summary>Gets or sets the method.</summary>
        /// <value>The method.</value>
        public virtual string Method { get; set; }

        /// <summary>Gets or sets the remote user.  These fields are automatically mapped from the TCP level server variables.</summary>
        /// <value>The remote user.</value>
        public virtual ApiRemoteUser RemoteUser { get; set; }

        /// <summary>Gets or sets the request date.</summary>
        /// <value>The request date.</value>
        public virtual DateTime? RequestDate { get; set; }

        /// <summary>Gets or sets the request identifier.</summary>
        /// <value>The request identifier.</value>
        public virtual string RequestIdentifier { get; set; } = Guid.NewGuid().ToString();

        /// <summary>Gets or sets the request URI.</summary>
        /// <value>The request URI.</value>
        public virtual string RequestUri { get; set; }

        /// <summary>Gets or sets the invocation context.</summary>
        /// <value>The invocation context.</value>
        public virtual ApiInvocationContext InvocationContext { get; set; }

        /// <summary>Gets or sets the query variables.</summary>
        /// <value>The query variables.</value>
        public virtual Dictionary<string, string> QueryVariables { get; set; } = new Dictionary<string, string>();

        /// <summary>Gets or sets the protocol.</summary>
        /// <value>The protocol.</value>
        public virtual string Protocol { get; set; }

        /// <summary>Gets or sets the path.</summary>
        /// <value>The path.</value>
        public virtual string Path { get; set; }

        /// <summary>Gets or sets the headers.</summary>
        /// <value>The headers.</value>
        public virtual List<ApiHeader> Headers { get; set; } = new List<ApiHeader>();

        /// <summary>Gets or sets the body.</summary>
        /// <value>The body.</value>
        public virtual Stream Body { get; set; }

        /// <summary>Dumps this instance.</summary>
        /// <returns></returns>
        public string Dump()
        {
            var builder = new StringBuilder();

            builder.AppendLine($"{this.Method.ToUpper()} {this.RequestUri} {this.Protocol}");
            builder.AppendLine("");

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
    public static class ApiRequestInfoExtensionMethods
    {
        /// <summary>
        /// Determines whether [is cors preflight request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is cors preflight request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCorsPreflightRequest(this ApiRequestInfo request)
        {
            if (request?.Method?.ToUpper() == "OPTIONS")
            {
                if (!string.IsNullOrWhiteSpace(request?.CrossOriginRequest?.Origin))
                {
                    if (!string.IsNullOrWhiteSpace(request.CrossOriginRequest.AccessControlRequestMethod))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is head request].
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>true</c> if [is head request] [the specified request]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsHeadRequest(this ApiRequestInfo request)
        {
            if (request?.Method?.ToUpper() == "HEAD")
            {
                return true;
            }

            return false;
        }
    }
}