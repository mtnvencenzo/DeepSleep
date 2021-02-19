namespace DeepSleep
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IUriRouteResolver
    {
        /// <summary>Resolves the route.</summary>
        /// <param name="template">The template.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        Task<RouteMatch> ResolveRoute(string template, string uri);

        /// <summary>Matches the route.</summary>
        /// <param name="routes">The routes.</param>
        /// <param name="method">The method.</param>
        /// <param name="requestPath">The request path.</param>
        /// <returns></returns>
        Task<ApiRoutingItem> MatchRoute(
            IApiRoutingTable routes,
            string method,
            string requestPath);
    }
}
