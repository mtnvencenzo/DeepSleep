using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public enum ApiValidationState
    {
        /// <summary>The not attempted</summary>
        NotAttempted = 0,

        /// <summary>The succeeded</summary>
        Succeeded = 1,

        /// <summary>The failed</summary>
        Failed = 2,

        /// <summary>The validating</summary>
        Validating = 3
    }
}
