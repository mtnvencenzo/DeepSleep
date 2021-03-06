﻿namespace DeepSleep
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("IsMatch = {IsMatch}")]
    public class RouteMatch
    {
        /// <summary>Gets or sets a value indicating whether this instance is match.</summary>
        /// <value><c>true</c> if this instance is match; otherwise, <c>false</c>.</value>
        public bool IsMatch { get; set; }

        /// <summary>Gets the route variables.</summary>
        /// <value>The route variables.</value>
        public Dictionary<string, string> RouteVariables { get; set; } = new Dictionary<string, string>();
    }
}
