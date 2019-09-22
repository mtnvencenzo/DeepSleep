using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>Defines how a particular HTTP resource should be handled within HTTp Cacheing.</summary>
    [Serializable]
    public class HttpCacheDirective
    {
        /// <summary>Gets or sets the cacheability.</summary>
        /// <value>The cacheability.</value>
        public HttpCacheType Cacheability { get; set; }

        /// <summary>Gets or sets the expiration.</summary>
        /// <value>The expiration.</value>
        public int ExpirationSeconds { get; set; }

        /// <summary>Gets or sets the cache location.</summary>
        /// <value>The cache location.</value>
        public HttpCacheLocation CacheLocation { get; set; }
    }
}
