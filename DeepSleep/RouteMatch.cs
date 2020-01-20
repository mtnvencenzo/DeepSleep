namespace DeepSleep
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class RouteMatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteMatch"/> class.
        /// </summary>
        public RouteMatch()
        {
            RouteVariables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is match.
        /// </summary>
        /// <value><c>true</c> if this instance is match; otherwise, <c>false</c>.</value>
        public bool IsMatch { get; set; }

        /// <summary>Gets the route variables.</summary>
        /// <value>The route variables.</value>
        public Dictionary<string, string> RouteVariables { get; set; }
    }
}
