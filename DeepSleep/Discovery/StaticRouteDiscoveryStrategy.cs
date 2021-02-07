namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.IDeepSleepDiscoveryStrategy" />
    public class StaticRouteDiscoveryStrategy : IDeepSleepDiscoveryStrategy
    {
        private readonly List<DeepSleepRouteRegistration> registrations;

        /// <summary>Initializes a new instance of the <see cref="StaticRouteDiscoveryStrategy"/> class.
        /// </summary>
        public StaticRouteDiscoveryStrategy()
        {
            this.registrations = new List<DeepSleepRouteRegistration>();
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public virtual Task<IList<DeepSleepRouteRegistration>> DiscoverRoutes(IServiceProvider serviceProvider)
        {
            return Task.FromResult(this.registrations as IList<DeepSleepRouteRegistration>);
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
        public StaticRouteDiscoveryStrategy AddRoute(string template, IList<string> httpMethods, Type controller, string endpoint, IDeepSleepRequestConfiguration config)
        {
            var registration = new DeepSleepRouteRegistration(
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
