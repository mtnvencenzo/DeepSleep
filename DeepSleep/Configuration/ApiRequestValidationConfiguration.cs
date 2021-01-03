namespace DeepSleep.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestValidationConfiguration
    {
        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public long? MaxRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int? MaxRequestUriLength { get; set; }

        /// <summary>Gets or sets the maximum length of the header.</summary>
        /// <value>The maximum length of the header.</value>
        public int? MaxHeaderLength { get; set; }

        /// <summary>Whteher or not to allow a request body on POST, PATCH, PUT requests when no body model type is defined on the processing endpoint method.
        /// {true} to allow a request body.  If {false}, the api will response with an http 413 Payload Too Large response if a body model type has not been defined.  The default value for this is {false}</summary>
        /// <value>The allow request body when no model defined.</value>
        public bool? AllowRequestBodyWhenNoModelDefined { get; set; }

        /// <summary>Whether or not to require a Content-Length header on POST, PATCH, PUT requests.
        /// {true} to require a Content-Length header. If {true}, the api will response with an http 411 Length Required response.  The default value for this is {true}.
        /// </summary>
        /// <value>The require content length on request body requests.</value>
        public bool? RequireContentLengthOnRequestBodyRequests { get; set; }
    }
}
