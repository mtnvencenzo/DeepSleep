namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteCrossOriginAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteCrossOriginAttribute"/> class.</summary>
        /// <param name="allowCredentials">The allow credentials.</param>
        /// <param name="allowedHeaders">The allowed headers.</param>
        /// <param name="allowedOrigins">The allowed origins.</param>
        /// <param name="exposeHeaders">The expose headers.</param>
        /// <param name="maxAgeSeconds">The maximum age seconds.</param>
        public ApiRouteCrossOriginAttribute(bool allowCredentials = true, string[] allowedHeaders = null, string[] allowedOrigins = null, string[] exposeHeaders = null, int maxAgeSeconds = 600)
        {
            this.AllowCredentials = allowCredentials;
            this.AllowedHeaders = allowedHeaders;
            this.AllowedOrigins = allowedOrigins;
            this.ExposeHeaders = exposeHeaders;
            this.MaxAgeSeconds = maxAgeSeconds;
        }

        /// <summary>Gets the allow credentials.</summary>
        /// <value>The allow credentials.</value>
        public bool? AllowCredentials { get; private set; }

        /// <summary>Gets the allowed headers.</summary>
        /// <value>The allowed headers.</value>
        public string[] AllowedHeaders { get; private set; }

        /// <summary>Gets the allowed origins.</summary>
        /// <value>The allowed origins.</value>
        public string[] AllowedOrigins { get; private set; }

        /// <summary>Gets the expose headers.</summary>
        /// <value>The expose headers.</value>
        public string[] ExposeHeaders { get; private set; }

        /// <summary>Gets the maximum age seconds.</summary>
        /// <value>The maximum age seconds.</value>
        public int? MaxAgeSeconds { get; private set; }
    }
}
