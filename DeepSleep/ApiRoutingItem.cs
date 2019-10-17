namespace DeepSleep
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRoutingItem
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRoutingItem"/> class.
        /// </summary>
        public ApiRoutingItem()
        {
            VariablesList = new List<string>();
            RouteVariables = new Dictionary<string, string>();
        }

        #endregion

        #region Private Fields

        private string _method;

        #endregion

        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; set; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get => _method; set => _method = value?.ToUpper(); }

        /// <summary>Gets or sets the endpoint location.</summary>
        /// <value>The endpoint location.</value>
        public ApiEndpointLocation EndpointLocation { get; set; }

        /// <summary>Gets or sets the configuration.</summary>
        /// <value>The configuration.</value>
        public ApiResourceConfig Config { get; set; }

        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets or sets the template variables.</summary>
        /// <value>The template variables.</value>
        public List<string> VariablesList { get; set; }

        /// <summary>Gets or sets the route variables.</summary>
        /// <value>The route variables.</value>
        public Dictionary<string, string> RouteVariables { get; set; }
    }
}
