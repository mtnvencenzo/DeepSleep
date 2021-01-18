namespace DeepSleep.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines how a particular HTTP resource should respond to cross origin resource sharing (CORS) requests.
    /// </summary>
    /// <remarks>
    /// Properties of this configuration, when applied on a specific route will override the default request configuration.  If not specified either on the route or on the default configuration
    /// the values defined at the internal system configuration will apply.  See each properties documentation to find out the system configuration default value.
    /// <para>
    /// [null] property values will allow the default or system configuration's value to be applied, otherwise non-null property value will override all lower level configurations.
    /// </para>
    /// <para>
    /// [see-also] Mozilla (Cross-Origin Resource Sharing (CORS): <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS"/>)
    /// </para>
    /// </remarks>
    public class ApiCrossOriginConfiguration
    {
        /// <summary>Gets or sets the Access-Control-Expose-Headers to respond with during preflight CORS requests.</summary>
        /// <remarks>
        /// The default configuration for this value is an empty list and therefore no Access-Control-Expose-Headers are supplied.
        /// <para>
        /// [see-also] Mozilla (Access-Control-Expose-Headers: <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS#access-control-expose-headers"/>)
        /// </para>
        /// </remarks>
        /// <value>The Access-Control-Expose-Headers configuration value.</value>
        public IList<string> ExposeHeaders { get; set; }

        /// <summary>Gets or sets the allowed origins to use in the Access-Control-Allow-Origin header when responding to CORS requests.</summary>
        /// <remarks>
        /// The deep sleep framework does not send out the value '*' even if configured as such.  When an asterisk is configured,  the framework will respond 
        /// with the exact value supplied in the request Origin header.  If specific origins are configured, only when the supplied request Origin header value matches one of the
        /// configured values will the response Access-Control-Allow-Origin header contain the origin value.  If the request Origin header value does not match any configured value
        /// then the Access-Control-Allow-Origin header value will be empty.
        /// <para>
        /// The default configuration for this value is '*'.
        /// </para>
        /// <para>
        /// [see-also] Mozilla (Access-Control-Allow-Origin: <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS#access-control-allow-origin"/>)
        /// </para>
        /// </remarks>
        /// <value>The allowed origins to use when formulating the response Access-Control-Allow-Origin header.</value>
        public IList<string> AllowedOrigins { get; set; }

        /// <summary>Gets or sets a value indicating whether or not a response can be exposed when credentials are provided in the request.</summary>
        /// <remarks>
        /// When used as part of a response to a preflight request, this indicates whether or not the actual request can be made using credentials.
        /// <para>
        /// The default configuration for this value is <c>true</c>.
        /// </para>
        /// <para>
        /// [see-also] Mozilla (Access-Control-Allow-Credentials: <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS#access-control-allow-credentials"/>)
        /// </para>
        /// </remarks>
        /// <value>Whether or not a response can be exposed when credentials are provided.</value>
        public bool? AllowCredentials { get; set; }

        /// <summary>Gets or sets a value indicating the number of seconds a preflight response can be cached.</summary>
        /// <remarks>
        /// Used when formulating the Access-Control-Max-Age header in preflight CORS responses.
        /// <para>
        /// The default configuration for this value is <c>0</c>.
        /// </para>
        /// <para>
        /// [see-also] Mozilla (Access-Control-Max-Age: <see href="https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS#access-control-max-age"/>)
        /// </para>
        /// </remarks>
        /// <value>The number of seconds a preflight response can be cached for.</value>
        public int? MaxAgeSeconds { get; set; }

        /// <summary>Gets or sets the allowed headers.</summary>
        /// <value>The allowed headers.</value>
        public IList<string> AllowedHeaders { get; set; }
    }
}
