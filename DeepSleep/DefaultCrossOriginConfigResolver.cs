namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.ICrossOriginConfigResolver" />
    public class DefaultCrossOriginConfigResolver : ICrossOriginConfigResolver
    {
        private List<string> exposeHeaders;
        private List<string> allowedOrigins;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCrossOriginConfigResolver"/> class.
        /// </summary>
        public DefaultCrossOriginConfigResolver()
        {
            exposeHeaders = new List<string>();
            allowedOrigins = new List<string>();
        }

        /// <summary>Resolves the configuration.</summary>
        /// <returns></returns>
        public Task<CrossOriginConfiguration> ResolveConfig()
        {
            TaskCompletionSource<CrossOriginConfiguration> source = new TaskCompletionSource<CrossOriginConfiguration>();
            source.SetResult(new CrossOriginConfiguration
            {
                ExposeHeaders = exposeHeaders,
                AllowedOrigins = allowedOrigins
            });

            return source.Task;
        }

        /// <summary>Adds the expose headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public DefaultCrossOriginConfigResolver AddExposeHeaders(IEnumerable<string> headers)
        {
            exposeHeaders.AddRange(headers);
            return this;
        }

        /// <summary>Adds the allowed origins.</summary>
        /// <param name="origins">The origins.</param>
        /// <returns></returns>
        public DefaultCrossOriginConfigResolver AddAllowedOrigins(IEnumerable<string> origins)
        {
            allowedOrigins.AddRange(origins);
            return this;
        }
    }
}
