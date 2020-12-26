namespace DeepSleep
{
    using DeepSleep.Configuration;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("[{HttpMethod?.ToUpper()}] {Template}")]
    public class ApiRoutingItem
    {
        private string method;

        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; set; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get => method; set => method = value?.ToUpper(); }

        /// <summary>Gets or sets the endpoint location.</summary>
        /// <value>The endpoint location.</value>
        public ApiEndpointLocation Location { get; set; }

        /// <summary>Gets or sets the configuration.</summary>
        /// <value>The configuration.</value>
        public IApiRequestConfiguration Configuration { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name => $"{HttpMethod}_{Template}";

        /// <summary>Gets or sets the route variables.</summary>
        /// <value>The route variables.</value>
        public IDictionary<string, string> RouteVariables { get; set; } = new Dictionary<string, string>();
    }
}
