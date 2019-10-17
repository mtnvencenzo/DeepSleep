namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRoutingTable
    {
        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        IEnumerable<ApiRoutingItem> GetRoutes();

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        Task<IApiRoutingTable> AddRoute(string name, string template, string httpMethod, Type controller, string endpoint);
    }
}
