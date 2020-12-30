namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.IRouteDiscoveryStrategy" />
    public class StaticRouteDiscoveryStrategy : IRouteDiscoveryStrategy
    {
        private readonly List<ApiRouteRegistration> registrations;

        /// <summary>Initializes a new instance of the <see cref="StaticRouteDiscoveryStrategy"/> class.
        /// </summary>
        public StaticRouteDiscoveryStrategy()
        {
            this.registrations = new List<ApiRouteRegistration>();
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public virtual Task<IList<ApiRouteRegistration>> DiscoverRoutes(IServiceProvider serviceProvider)
        {
            return Task.FromResult(this.registrations as IList<ApiRouteRegistration>);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public StaticRouteDiscoveryStrategy AddRoute(string template, string httpMethod, Type controller, string endpoint)
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
        public StaticRouteDiscoveryStrategy AddRoute(string template, string httpMethod, Type controller, string endpoint, IApiRequestConfiguration config)
        {
            if (this.registrations.Exists(r => string.Equals(r.Template, template, StringComparison.OrdinalIgnoreCase) &&
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

            var item = new ApiRouteRegistration
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

            registrations.Add(item);
            return this;
        }
    }
}
