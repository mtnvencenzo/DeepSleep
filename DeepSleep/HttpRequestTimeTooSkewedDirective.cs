﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>Defines how a particular HTTP resource should be handle request time skewing.</summary>
    [Serializable]
    public class HttpRequestTimeTooSkewedDirective
    {
        /// <summary>Gets or sets the cacheability.</summary>
        /// <value>The cacheability.</value>
        public bool Enabled { get; set; }

        /// <summary>Gets or sets the threshold in minutes to determine if the time is too skewed.</summary>
        /// <value>The threshold in minutes.</value>
        public int Threshold { get; set; }
    }
}
