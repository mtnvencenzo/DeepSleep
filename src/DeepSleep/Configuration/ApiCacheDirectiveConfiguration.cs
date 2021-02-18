namespace DeepSleep.Configuration
{
    /// <summary>
    /// Defines how a particular HTTP resource response should define its HTTP caching headers.
    /// </summary>
    /// <remarks>
    /// Properties of this configuration, when applied on a specific route will override the default request configuration.  If not specified either on the route or on the default configuration
    /// the values defined at the internal system configuration will apply.  See each properties documentation to find out the system configuration default value.
    /// <para>
    /// [null] property values will allow the default or system configuration's value to be applied, otherwise non-null property value will override all lower level configurations.
    /// </para>
    /// <para>
    /// [see-also] Hypertext Transfer Protocol (13.4 Response Cacheability: <see href="https://tools.ietf.org/html/rfc2616#section-13.4"/>)
    /// </para>
    /// </remarks>
    public class ApiCacheDirectiveConfiguration
    {
        /// <summary>Gets or sets the cacheability value which defines whether or not the request can be cached by proxy servers or browsers.</summary>
        /// <remarks>
        /// The default configuration for this value is <see cref="HttpCacheType.NoCache"/>
        /// <para>
        /// [see-also] Hypertext Transfer Protocol (14.9 Cache-Control: <see href="https://tools.ietf.org/html/rfc2616#section-14.9"/>)
        /// </para>
        /// </remarks>
        /// <value>The cacheability of the route.</value>
        public HttpCacheType? Cacheability { get; set; }

        /// <summary>Gets or sets the number of seconds relative to the response Date header that is used as the Expires and Max-Age cache control values. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The seconds defined here will be added to a date value equaling the value used in the response Date header.
        /// The final adjusted date value will be applied in the Expires response header.
        /// </para>
        /// <para>
        /// The exaxt seconds defined here are also used as the max-age parameter of the Cache-Control response header.  Note, when non-cacheable responses
        /// are sent, the max-age will always be zero regardless of the configured <see cref="ExpirationSeconds"/>.
        /// </para>
        /// <para>
        /// The default configuration for this value is -1.
        /// </para>
        /// <para>
        /// [see-also] Hypertext Transfer Protocol (14.21 Expires: <see href="https://tools.ietf.org/html/rfc2616#section-14.21"/>)
        /// </para>
        /// </remarks>
        /// <value>The expiration seconds.</value>
        public int? ExpirationSeconds { get; set; }

        /// <summary>Gets or sets the cache location value that determines which the caches are allowed to use the cached resource for subsequent requests.
        /// This value is only used when the <see cref="HttpCacheType"/> property is set to <see cref="HttpCacheType.Cacheable"/>.</summary>
        /// <remarks>
        /// The default configuration for this value is <see cref="HttpCacheLocation.Private"/>
        /// <para>
        /// [see-also] Hypertext Transfer Protocol (14.9.1 What is Cacheable: <see href="https://tools.ietf.org/html/rfc2616#section-14.9.1"/>)
        /// </para>
        /// </remarks>
        /// <value>The cache location value.</value>
        public HttpCacheLocation? CacheLocation { get; set; }

        /// <summary>Gets or sets the Vary header value to use with cacheable responses.
        /// This value is only used when the <see cref="HttpCacheType"/> property is set to <see cref="HttpCacheType.Cacheable"/>.</summary>
        /// <remarks>
        /// The default configuration for this value is: 'Accept, Accept-Encoding, Accept-Language'.
        /// <para>
        /// [see-also] Hypertext Transfer Protocol (14.44 Vary: <see href="https://tools.ietf.org/html/rfc2616#section-14.44"/>)
        /// </para>
        /// </remarks>
        /// <value>The Vary header value.</value>
        public string VaryHeaderValue { get; set; }
    }
}
