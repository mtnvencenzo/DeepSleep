using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ApiRequestPipelineHookPlacements
    {
        /// <summary>The none</summary>
        None = 0,

        /// <summary>The before</summary>
        Before = 1,

        /// <summary>The after</summary>
        After = 2,

        /// <summary>The before and after</summary>
        BeforeAndAfter = 3
    }
}
