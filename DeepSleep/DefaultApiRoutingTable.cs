namespace DeepSleep
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultApiRoutingTable : IApiRoutingTable
    {
        private readonly List<ApiRoutingItem> routes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApiRoutingTable"/> class.
        /// </summary>
        public DefaultApiRoutingTable()
        {
            routes = new List<ApiRoutingItem>();
        }

        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        public IList<ApiRoutingItem> GetRoutes()
        {
            return routes;
        }

        /// <summary>Adds the route.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public IApiRoutingTable AddRoute(string template, string httpMethod, Type controller, string endpoint)
        {
            return AddRoute(template, httpMethod, controller, endpoint, null);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Route '{httpMethod} {template}' already has been added.
        /// or
        /// Controller must be specified
        /// or
        /// </exception>
        /// <exception cref="MissingMethodException"></exception>
        public IApiRoutingTable AddRoute(string template, string httpMethod, Type controller, string endpoint, IApiRequestConfiguration config)
        {
            if (this.routes.Exists(r => string.Equals(r.Template, template, StringComparison.OrdinalIgnoreCase) &&
                 string.Equals(r.HttpMethod, httpMethod, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception($"Route '{httpMethod} {template}' already has been added.");
            }

            if (controller == null)
            {
                throw new Exception("Controller must be specified");
            }

            var method = controller.GetMethod(endpoint, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            if (method == null)
            {
                throw new MissingMethodException(string.Format("Endpoint '{0}' does not exist on controller '{1}'", endpoint, controller.FullName));
            }

            if (string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new Exception(string.Format("Http method not specified", endpoint, controller.FullName));
            }

            var item = new ApiRoutingItem
            {
                Template = template,
                HttpMethod = httpMethod.ToUpper(),
                Configuration = config,
                Location = new ApiEndpointLocation
                {
                    Controller = Type.GetType(controller.AssemblyQualifiedName),
                    Endpoint = endpoint,
                    HttpMethod = httpMethod.ToUpper()
                }
            };

            routes.Add(item);
            return this;
        }
    }
}