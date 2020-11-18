namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IUriRouteResolver" />
    public class DefaultRouteResolver : IUriRouteResolver
    {
        #region Helper Methods

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
        
        #endregion

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

            var match = new RouteMatch
            {
                IsMatch = false
            };

            if(IsMatch(formattedRoute, decodedUri))
            {
                match.IsMatch = true;
                match.RouteVariables = GetRouteVariables(formattedRoute, decodedUri);
            }

            TaskCompletionSource<RouteMatch> source = new TaskCompletionSource<RouteMatch>();
            source.SetResult(match);
            return source.Task;
        }
    }
}
