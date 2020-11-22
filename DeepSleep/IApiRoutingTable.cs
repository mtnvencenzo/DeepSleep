namespace DeepSleep
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRoutingTable
    {
        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        IList<ApiRoutingItem> GetRoutes();

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        IApiRoutingTable AddRoute(string name, string template, string httpMethod, Type controller, string endpoint);

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        IApiRoutingTable AddRoute(string name, string template, string httpMethod, Type controller, string endpoint, IApiRequestConfiguration config);
    }
}
