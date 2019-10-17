namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultApiRoutingTable : IApiRoutingTable
    {
        #region Constructors & Initialization

        private List<ApiRoutingItem> _routes;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApiRoutingTable"/> class.
        /// </summary>
        public DefaultApiRoutingTable()
        {
            _routes = new List<ApiRoutingItem>();
        }

        #endregion

        #region Helper Methods

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

        #endregion

        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        public IEnumerable<ApiRoutingItem> GetRoutes()
        {
            return _routes;
        }

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        /// <exception cref="System.MissingMethodException"></exception>
        public async Task<IApiRoutingTable> AddRoute(string name, string template, string httpMethod, Type controller, string endpoint)
        {
            return await AddRoute(name, template, httpMethod, controller, endpoint, null as ApiResourceConfig).ConfigureAwait(false);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public async Task<IApiRoutingTable> AddRoute(string name, string template, string httpMethod, Type controller, string endpoint, ApiResourceConfig config)
        {
            return await AddRoute(name, template, httpMethod, controller, endpoint, () => 
            {
                TaskCompletionSource<ApiResourceConfig> source = new TaskCompletionSource<ApiResourceConfig>();
                source.SetResult(config);
                return source.Task;
            }).ConfigureAwait(false);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="name">The name.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="MissingMethodException"></exception>
        /// <exception cref="Exception">
        /// </exception>
        public async Task<IApiRoutingTable> AddRoute(string name, string template, string httpMethod, Type controller, string endpoint, Func<Task<ApiResourceConfig>> config)
        {
            var method = controller.GetMethod(endpoint, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            if (method == null)
            {
                throw new MissingMethodException(string.Format("Endpoint '{0}' does not exist on controller '{1}'", endpoint, controller.FullName));
            }

            if (config == null)
            {
                throw new Exception(string.Format("Configuration must be supplied", endpoint, controller.FullName));
            }

            var configuration = await config().ConfigureAwait(false);

            if (configuration == null)
            {
                throw new Exception(string.Format("Configuration was not supplied", endpoint, controller.FullName));
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
                Config = configuration,
                EndpointLocation = new ApiEndpointLocation
                {
                    Controller = Type.GetType(controller.AssemblyQualifiedName),
                    Endpoint = endpoint,
                    HttpMethod = httpMethod.ToUpper()
                }
            };

            _routes.Add(item);
            return this;
        }
    }
}