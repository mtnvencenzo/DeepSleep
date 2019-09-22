using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiRequestPipelineBuilder
    {
        /// <summary>Builds this instance.</summary>
        /// <returns></returns>
        IApiRequestPipeline Build();
    }
}
