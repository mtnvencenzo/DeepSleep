namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRoutingInfo
    {
        /// <summary>Gets or sets the template information.</summary>
        /// <value>The template information.</value>
        public virtual ApiRoutingTemplate TemplateInfo { get; set; }

        /// <summary>Gets or sets the routing item.</summary>
        /// <value>The routing item.</value>
        public virtual ApiRoutingItem RoutingItem { get; set; }
    }
}
