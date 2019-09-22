using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.ICrossOriginConfigResolver" />
    public class DefaultCrossOriginConfigResolver : ICrossOriginConfigResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCrossOriginConfigResolver"/> class.
        /// </summary>
        public DefaultCrossOriginConfigResolver()
        {
            _exposeHeaders = new List<string>();
        }

        private List<string> _exposeHeaders;

        /// <summary>Resolves the configuration.</summary>
        /// <returns></returns>
        public Task<CrossOriginConfiguration> ResolveConfig()
        {
            TaskCompletionSource<CrossOriginConfiguration> source = new TaskCompletionSource<CrossOriginConfiguration>();
            source.SetResult(new CrossOriginConfiguration
            {
                ExposeHeaders = _exposeHeaders
            });

            return source.Task;
        }

        /// <summary>Adds the expose headers.</summary>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        public ICrossOriginConfigResolver AddExposeHeaders(IEnumerable<string> headers)
        {
            _exposeHeaders.AddRange(headers);
            return this;
        }
    }
}
