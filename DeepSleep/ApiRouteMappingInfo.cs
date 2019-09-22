using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRouteMappingInfo
    {
        /// <summary>Gets or sets the route.</summary>
        /// <value>The route.</value>
        public string Route { get; set; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; set; }

        /// <summary>Gets or sets the type of the contraller.</summary>
        /// <value>The type of the contraller.</value>
        public string ContrallerType { get; set; }

        /// <summary>Gets or sets the name of the method.</summary>
        /// <value>The name of the method.</value>
        public string MethodName { get; set; }
    }
}
