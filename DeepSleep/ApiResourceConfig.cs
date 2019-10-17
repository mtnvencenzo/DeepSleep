namespace DeepSleep
{
    using System;
    using System.Collections.Generic;

    /// <summary>Defines the configuration settings for a particular service resource.</summary>
    public class ApiResourceConfig
    {
        /// <summary>Gets or sets the identity.</summary>
        /// <value>The identity.</value>
        public string ResourceId { get; set; }

        /// <summary>Gets or sets a value indicating whether [allow anonymous].</summary>
        /// <value><c>true</c> if [allow anonymous]; otherwise, <c>false</c>.</value>
        public bool AllowAnonymous { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int MinRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int MaxRequestLength { get; set; }

        /// <summary>Gets or sets the maximum request length in bytes.</summary>
        /// <value>The length of the max allowed bytes.</value>
        public int MaxRequestUriLength { get; set; }

        /// <summary>Gets or sets the caching directive.</summary>
        /// <value>The caching directive.</value>
        public HttpCacheDirective CacheDirective { get; set; }

        /// <summary>Gets or sets a value indicating whether [enable request time too skewed handling].</summary>
        /// <value><c>true</c> if [enable request time too skewed handling]; otherwise, <c>false</c>.</value>
        public HttpRequestTimeTooSkewedDirective RequestTimeTooSkewedDirective { get; set; }

        /// <summary>Gets or sets a value indicating whether [require SSL].</summary>
        /// <value><c>true</c> if [require SSL]; otherwise, <c>false</c>.</value>
        public bool RequireSSL { get; set; }

        /// <summary>Gets or sets a value indicating whether the resource is deprecated.</summary>
        /// <value><c>true</c> if deprecated; otherwise, <c>false</c>.</value>
        public bool Deprecated { get; set; }

        /// <summary>Gets or sets the fall back language.</summary>
        /// <value>The fall back language.</value>
        public string FallBackLanguage { get; set; }

        /// <summary>Gets or sets the supported languages.</summary>
        /// <value>The supported languages.</value>
        public IEnumerable<string> SupportedLanguages { get; set; }
    }
}