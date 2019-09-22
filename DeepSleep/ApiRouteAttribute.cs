using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteAttribute" /> class.</summary>
        /// <param name="route">The route.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        public ApiRouteAttribute(string route, string httpMethod)
        {
            Route = route;
            HttpMethod = httpMethod;
        }

        /// <summary>Gets the route.</summary>
        /// <value>The route.</value>
        public string Route { get; private set; }

        /// <summary>Gets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; private set; }
    }
}
