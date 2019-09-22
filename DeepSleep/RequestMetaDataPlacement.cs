﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public enum RequestMetaDataPlacement
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The standard header
        /// </summary>
        StandardHeader = 1,

        /// <summary>
        /// The custom header
        /// </summary>
        CustomHeader = 2,

        /// <summary>
        /// The query string
        /// </summary>
        QueryString = 3
    }
}
