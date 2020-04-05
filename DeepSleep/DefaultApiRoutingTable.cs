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
        public IEnumerable<ApiRoutingItem> GetRoutes()
        {
            return routes;
        }

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        /// <exception cref="System.MissingMethodException"></exception>
        public IApiRoutingTable AddRoute(string name, string template, string httpMethod, Type controller, string endpoint)
        {
            return AddRoute(name, template, httpMethod, controller, endpoint, null);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public IApiRoutingTable AddRoute(string name, string template, string httpMethod, Type controller, string endpoint, IApiRequestConfiguration config)
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
                Name = name,
                HttpMethod = httpMethod.ToUpper(),
                VariablesList = GetTemplateVariables(template),
                Config = config,
                EndpointLocation = new ApiEndpointLocation
                {
                    Controller = Type.GetType(controller.AssemblyQualifiedName),
                    Endpoint = endpoint,
                    HttpMethod = httpMethod.ToUpper()
                }
            };

            routes.Add(item);
            return this;
        }

        /// <summary>Gets the template variables.</summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        private List<string> GetTemplateVariables(string template)
        {
            List<string> vars = new List<string>();

            string[] parts = template.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in parts)
            {
                if (part.StartsWith("{"))
                {
                    vars.Add(part);
                }
            }

            return vars;
        }

    }
}