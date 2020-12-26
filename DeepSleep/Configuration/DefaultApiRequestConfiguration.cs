namespace DeepSleep.Configuration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Configuration.IApiRequestConfiguration" />
    public class DefaultApiRequestConfiguration : IApiRequestConfiguration
    {
        /// <summary>Gets or sets the cross origin configuration.</summary>
        /// <value>The cross origin configuration.</value>
        public ApiCrossOriginConfiguration CrossOriginConfig { get; set; }

        /// <summary>Gets or sets the header validation configuration.</summary>
        /// <value>The header validation configuration.</value>
        public ApiHeaderValidationConfiguration HeaderValidationConfig { get; set; }

        /// <summary>Gets or sets the API error response provider.</summary>
        /// <value>The API error response provider.</value>
        public Func<IServiceProvider, IApiErrorResponseProvider> ApiErrorResponseProvider { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        public bool? AllowAnonymous { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public long? MaxRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int? MaxRequestUriLength { get; set; }

        /// <summary>Gets or sets the caching directive.</summary>
        /// <value>The caching directive.</value>
        public HttpCacheDirective CacheDirective { get; set; }

        /// <summary>Gets or sets a value indicating whether the resource is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        public bool? Deprecated { get; set; }

        /// <summary>Whether or not to allow a request body on POST, PATCH, PUT requests when no body model type is defined on the processing endpoint method.
        /// {true} to allow a request body.  If {false}, the api will response with an http 413 Payload Too Large response if a body model type has not been defined..
        /// </summary>
        /// <value>The allow request body when no model defined.</value>
        public bool? AllowRequestBodyWhenNoModelDefined { get; set; }

        /// <summary>Whether or not to require a Content-Length header on POST, PATCH, PUT requests.
        /// {true} to reuqire a Content-Length header, {false} otherwise.  The default value for this is {true}.
        /// </summary>
        /// <value>The require content length on request body requests.</value>
        public bool? RequireContentLengthOnRequestBodyRequests { get; set; }

        /// <summary>Gets or sets the fall back language.</summary>
        /// <value>The fall back language.</value>
        public string FallBackLanguage { get; set; }

        /// <summary>Gets or sets the supported languages.</summary>
        /// <value>The supported languages.</value>
        public IList<string> SupportedLanguages { get; set; }

        /// <summary>Gets or sets the supported authentication schemes.  If not provided all available schemes are supported.</summary>
        /// <value>The supported authentication schemes.</value>
        public IList<string> SupportedAuthenticationSchemes { get; set; }

        /// <summary>Gets or sets the supported authorization configuration.</summary>
        /// <value>The supported authorization configuration.</value>
        public ResourceAuthorizationConfiguration AuthorizationConfig { get; set; }

        /// <summary>Gets or sets the include request identifier header in response.</summary>
        /// <value>The include request identifier header in response.</value>
        public bool? IncludeRequestIdHeaderInResponse { get; set; }

        /// <summary>Gets or sets the enable head for get requests.</summary>
        /// <value>The enable head for get requests.</value>
        public bool? EnableHeadForGetRequests { get; set; }

        /// <summary>Gets or sets the read write configuration.</summary>
        /// <value>The read write configuration.</value>
        public ApiReadWriteConfiguration ReadWriteConfiguration { get; set; }
    }
}
