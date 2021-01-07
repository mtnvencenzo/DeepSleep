namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IUriRouteResolver" />
    public class DefaultRouteResolver : IUriRouteResolver
    {
        /// <summary>Resolves the route.</summary>
        /// <param name="template">The template.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<RouteMatch> ResolveRoute(string template, string uri)
        {
            string formattedRoute = (template ?? string.Empty).Trim();
            string decodedUri = WebUtility.UrlDecode(uri ?? string.Empty);

            if (!formattedRoute.StartsWith("/"))
            {
                formattedRoute = "/" + formattedRoute;
            }

            if (decodedUri.EndsWith("/") && !formattedRoute.EndsWith("/"))
            {
                formattedRoute += "/";
            }

            if (IsMatch(formattedRoute, decodedUri))
            {
                return Task.FromResult(new RouteMatch
                {
                    IsMatch = true,
                    RouteVariables = GetRouteVariables(formattedRoute, decodedUri)
                });
            }

            return Task.FromResult(new RouteMatch
            {
                IsMatch = false
            });
        }

        /// <summary>Matches the route.</summary>
        /// <param name="routes">The routes.</param>
        /// <param name="method">The method.</param>
        /// <param name="requestPath">The request path.</param>
        /// <returns></returns>
        public async Task<ApiRoutingItem> MatchRoute(
            IApiRoutingTable routes,
            string method,
            string requestPath)
        {
            var potentialRoutes = routes?.GetRoutes()
                .Where(r => r.Location != null)
                .Where(r => string.Compare(r.HttpMethod, method, true) == 0);

            potentialRoutes = potentialRoutes ?? new List<ApiRoutingItem>();

            RouteMatch result;
            foreach (var route in potentialRoutes)
            {
                result = await this.ResolveRoute(route.Template, requestPath).ConfigureAwait(false);

                if (result?.IsMatch ?? false)
                {
                    var newRoute = CloneRoutingItem(route);
                    newRoute.RouteVariables = result.RouteVariables;
                    return newRoute;
                }
            }

            return null;
        }

        /// <summary>Parses the template and uri to match uri variables with template variables
        /// </summary>
        /// <param name="formattedTemplate"></param>
        /// <param name="formattedUri"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetRouteVariables(string formattedTemplate, string formattedUri)
        {
            var vars = new Dictionary<string, string>();

            string[] templateParts = formattedTemplate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string[] uriParts = formattedUri.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            string part;
            string varName;
            for (int i = 0; i < templateParts.Length; i++)
            {
                part = templateParts[i];

                if (part.StartsWith("{"))
                {
                    varName = part
                        .Replace("{", string.Empty)
                        .Replace("}", string.Empty);

                    if (vars.ContainsKey(varName))
                    {
                        throw new Exception(string.Format("Route template '{0}' contains duplicate variable names for variable '{1}'", formattedTemplate, varName));
                    }

                    vars.Add(varName, uriParts[i]);
                }
            }

            return vars;
        }

        /// <summary>Compares the teplate and uri to see if they match (exludeding route variables)
        /// </summary>
        /// <param name="formattedTemplate"></param>
        /// <param name="formattedUri"></param>
        /// <returns></returns>
        private bool IsMatch(string formattedTemplate, string formattedUri)
        {
            if (string.Compare(formattedTemplate, formattedUri, true) == 0)
            {
                return true;
            }

            string[] templateParts = formattedTemplate.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            string[] uriParts = formattedUri.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            if (templateParts.Length != uriParts.Length)
            {
                return false;
            }

            string part;
            for (int i = 0; i < templateParts.Length; i++)
            {
                part = templateParts[i];

                if (part.StartsWith("{"))
                {
                    continue;
                }

                if (string.Compare(part, uriParts[i], true) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Clones the routing item.</summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private ApiRoutingItem CloneRoutingItem(ApiRoutingItem item)
        {
            var newitem = new ApiRoutingItem
            {
                Location = new ApiEndpointLocation(
                    controller: item.Location.Controller,
                    endpoint: item.Location.Endpoint,
                    httpMethod: item.Location.HttpMethod,
                    methodInfo: item.Location.MethodInfo,
                    bodyParameterType: item.Location.BodyParameterType,
                    uriParameterType: item.Location.UriParameterType,
                    simpleParameters: item.Location.SimpleParameters,
                    methodReturnType: item.Location.MethodReturnType),
                Template = item.Template,
                Configuration = item.Configuration,
                HttpMethod = item.HttpMethod,
                RouteVariables = new Dictionary<string, string>()
            };

            return newitem;
        }
    }
}
