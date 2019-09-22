using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public enum ApiRequestPipelineHookContinuation
    {
        /// <summary>The continue
        /// </summary>
        InvokeComponentAndContinue = 1,

        /// <summary>
        /// The invoke component and cancel
        /// </summary>
        InvokeComponentAndCancel = 2,

        /// <summary>
        /// The bypass component and continue
        /// </summary>
        BypassComponentAndContinue = 3,

        /// <summary>
        /// The by pass component and cancel
        /// </summary>
        ByPassComponentAndCancel = 4
    }
}
