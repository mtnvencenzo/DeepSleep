namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
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
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public StaticRouteDiscoveryStrategy AddRoute(string template, IList<string> httpMethods, Type controller, string endpoint)
        {
            return AddRoute(template, httpMethods, controller, endpoint, null);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public StaticRouteDiscoveryStrategy AddRoute(string template, IList<string> httpMethods, Type controller, string endpoint, IApiRequestConfiguration config)
        {
            var registration = new ApiRouteRegistration(
                template: template,
                httpMethods: httpMethods,
                controller: controller,
                endpoint: endpoint,
                config: config);

            registrations.Add(registration);
            return this;
        }
    }
}
