namespace DeepSleep.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Configuration.IApiRequestConfiguration" />
    public class DefaultApiRequestConfiguration : IApiRequestConfiguration
    {
        /// <summary>Gets or sets the HTTP configuration.</summary>
        /// <value>The HTTP configuration.</value>
        public ApiHttpConfiguration HttpConfig { get; set; }

        /// <summary>Gets or sets the cross origin configuration.</summary>
        /// <value>The cross origin configuration.</value>
        public CrossOriginConfiguration CrossOriginConfig { get; set; }

        /// <summary>Gets or sets the header validation configuration.</summary>
        /// <value>The header validation configuration.</value>
        public ApiHeaderValidationConfiguration HeaderValidationConfig { get; set; }

        /// <summary>Gets or sets the authorization configuration
        /// </summary>
        public ResourceAuthorizationConfiguration ResourceAuthorizationConfig { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        public bool? AllowAnonymous { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int? MinRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int? MaxRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int? MaxRequestUriLength { get; set; }

        /// <summary>Gets or sets the caching directive.</summary>
        /// <value>The caching directive.</value>
        public HttpCacheDirective CacheDirective { get; set; }

        /// <summary>Gets or sets a value indicating whether the resource is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        public bool? Deprecated { get; set; }

        /// <summary>Gets or sets the fall back language.</summary>
        /// <value>The fall back language.</value>
        public string FallBackLanguage { get; set; }

        /// <summary>Gets or sets the supported languages.</summary>
        /// <value>The supported languages.</value>
        public IList<string> SupportedLanguages { get; set; }

        /// <summary>Gets or sets the supported authentication schemes.  If not provided all available schemes are supported.</summary>
        /// <value>The supported authentication schemes.</value>
        public IList<string> SupportedAuthenticationSchemes { get; set; }

        /// <summary>Initializes this instance.</summary>
        /// <returns></returns>
        public IApiRequestConfiguration Init()
        {
            return new DefaultApiRequestConfiguration();
        }
    }
}
