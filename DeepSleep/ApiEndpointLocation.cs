using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ApiEndpointLocation
    {
        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        public Type Controller { get; set; }

        /// <summary>Gets or sets the endpoint.</summary>
        /// <value>The endpoint.</value>
        public string Endpoint { get; set; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; set; }
    }
}
