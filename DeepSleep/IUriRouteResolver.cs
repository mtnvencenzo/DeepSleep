using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeepSleep
{
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
