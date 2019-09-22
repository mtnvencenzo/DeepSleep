using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICrossOriginConfigResolver
    {
        /// <summary>Resolves the configuration.</summary>
        /// <returns></returns>
        Task<CrossOriginConfiguration> ResolveConfig();
    }
}
