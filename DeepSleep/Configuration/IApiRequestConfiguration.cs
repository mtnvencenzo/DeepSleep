namespace DeepSleep.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRequestConfiguration
    {
        /// <summary>Gets or sets the HTTP configuration.</summary>
        /// <value>The HTTP configuration.</value>
        ApiHttpConfiguration HttpConfig { get; set; }

        /// <summary>Gets or sets the cross origin configuration.</summary>
        /// <value>The cross origin configuration.</value>
        CrossOriginConfiguration CrossOriginConfig { get; set; }

        /// <summary>Gets or sets the header validation configuration.</summary>
        /// <value>The header validation configuration.</value>
        ApiHeaderValidationConfiguration HeaderValidationConfig { get; set; }

        /// <summary>Gets or sets the authorization configuration
        /// </summary>
        ResourceAuthorizationConfiguration ResourceAuthorizationConfig { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        bool? AllowAnonymous { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        int? MinRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        int? MaxRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        int? MaxRequestUriLength { get; set; }

        /// <summary>Gets or sets the caching directive.</summary>
        /// <value>The caching directive.</value>
        HttpCacheDirective CacheDirective { get; set; }

        /// <summary>Gets or sets a value indicating whether the resource is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        bool? Deprecated { get; set; }

        /// <summary>Gets or sets the fall back language.</summary>
        /// <value>The fall back language.</value>
        string FallBackLanguage { get; set; }

        /// <summary>Gets or sets the supported languages.</summary>
        /// <value>The supported languages.</value>
        IList<string> SupportedLanguages { get; set; }

        /// <summary>Gets or sets the supported authentication schemes.  If not provided all available schemes are supported.</summary>
        /// <value>The supported authentication schemes.</value>
        IList<string> SupportedAuthenticationSchemes { get; set; }

        /// <summary>I
        /// nitializes this instance.</summary>
        /// <returns></returns>
        IApiRequestConfiguration Init();
    }
}
