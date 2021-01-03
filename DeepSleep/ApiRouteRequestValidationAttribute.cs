namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteRequestValidationAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteRequestValidationAttribute"/> class.</summary>
        /// <param name="maxHeaderLength">Maximum length of the header.</param>
        /// <param name="maxRequestLength">Maximum length of the request.</param>
        /// <param name="maxRequestUriLength">Maximum length of the request URI.</param>
        /// <param name="requireContentLengthOnRequestBodyRequests">if set to <c>true</c> [require content length on request body requests].</param>
        /// <param name="allowRequestBodyWhenNoModelDefined">if set to <c>true</c> [allow request body when no model defined].</param>
        public ApiRouteRequestValidationAttribute(
            int maxHeaderLength = -1, 
            int maxRequestLength = -1, 
            int maxRequestUriLength = -1,
            bool requireContentLengthOnRequestBodyRequests = true,
            bool allowRequestBodyWhenNoModelDefined = false)
        {
            if (maxHeaderLength >= 0)
                this.MaxHeaderLength = maxHeaderLength;

            if (maxRequestLength >= 0)
                this.MaxRequestLength = maxRequestLength;

            if (maxRequestUriLength >= 0)
                this.MaxRequestUriLength = maxRequestUriLength;

            this.RequireContentLengthOnRequestBodyRequests = requireContentLengthOnRequestBodyRequests;
            this.AllowRequestBodyWhenNoModelDefined = allowRequestBodyWhenNoModelDefined;
        }

        /// <summary>Gets the maximum length of the header.</summary>
        /// <value>The maximum length of the header.</value>
        public int? MaxHeaderLength { get; private set;}

        /// <summary>Gets the maximum length of the request.</summary>
        /// <value>The maximum length of the request.</value>
        public int? MaxRequestLength { get; private set; }

        /// <summary>Gets the maximum length of the request URI.</summary>
        /// <value>The maximum length of the request URI.</value>
        public int? MaxRequestUriLength { get; private set; }

        /// <summary>Gets the require content length on request body requests.</summary>
        /// <value>The require content length on request body requests.</value>
        public bool? RequireContentLengthOnRequestBodyRequests { get; private set; }

        /// <summary>Gets the allow request body when no model defined.</summary>
        /// <value>The allow request body when no model defined.</value>
        public bool? AllowRequestBodyWhenNoModelDefined { get; private set; }
    }
}
