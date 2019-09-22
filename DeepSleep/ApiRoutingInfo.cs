using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ApiRoutingInfo
    {
        /// <summary>Gets or sets the template information.</summary>
        /// <value>The template information.</value>
        public ApiRoutingTemplate TemplateInfo { get; set; }

        /// <summary>Gets or sets the routing item.</summary>
        /// <value>The routing item.</value>
        public ApiRoutingItem RoutingItem { get; set; }
    }
}
