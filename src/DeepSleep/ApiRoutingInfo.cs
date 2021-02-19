namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiRoutingInfo
    {
        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public virtual ApiRoutingTemplate Template { get; set; }

        /// <summary>Gets or sets the route.</summary>
        /// <value>The route.</value>
        public virtual ApiRoutingItem Route { get; set; }
    }
}
