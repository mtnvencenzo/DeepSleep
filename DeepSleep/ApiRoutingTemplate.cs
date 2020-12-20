namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRoutingTemplate
    {
        #region Constructors & Initialization

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRoutingTemplate"/> class.
        /// </summary>
        public ApiRoutingTemplate()
        {
            VariablesList = new List<string>();
            EndpointLocations = new List<ApiEndpointLocation>();
        }

        #endregion

        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; set; }

        /// <summary>Gets or sets the endpoint locations.</summary>
        /// <value>The endpoint locations.</value>
        public List<ApiEndpointLocation> EndpointLocations { get; set; }

        /// <summary>Gets or sets the template variables.</summary>
        /// <value>The template variables.</value>
        public List<string> VariablesList { get; set; }
    }
}
