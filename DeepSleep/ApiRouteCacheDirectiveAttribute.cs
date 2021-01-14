namespace DeepSleep
{
    using DeepSleep.Configuration;
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteCacheDirectiveAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteCacheDirectiveAttribute"/> class.</summary>
        /// <param name="location">The location.</param>
        /// <param name="cacheability">The cacheability.</param>
        /// <param name="expirationSeconds">The expiration seconds.</param>
        /// <param name="varyHeaderValue">The vary header value.</param>
        public ApiRouteCacheDirectiveAttribute(
            HttpCacheLocation location = HttpCacheLocation.Private,
            HttpCacheType cacheability = HttpCacheType.NoCache,
            int expirationSeconds = -1,
            string varyHeaderValue = null)
        {
            this.Location = location;
            this.Cacheability = cacheability;
            this.ExpirationSeconds = expirationSeconds;
            this.VaryHeaderValue = varyHeaderValue;
        }

        /// <summary>Gets the location.</summary>
        /// <value>The location.</value>
        public HttpCacheLocation? Location { get; private set; }

        /// <summary>Gets the cacheability.</summary>
        /// <value>The cacheability.</value>
        public HttpCacheType? Cacheability { get; private set; }

        /// <summary>Gets the expiration seconds.</summary>
        /// <value>The expiration seconds.</value>
        public int? ExpirationSeconds { get; private set; }

        /// <summary>Gets the vary header value.</summary>
        /// <value>The vary header value.</value>
        public string VaryHeaderValue { get; private set; }
    }
}
