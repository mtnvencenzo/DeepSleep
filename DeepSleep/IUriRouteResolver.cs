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
    }
}
