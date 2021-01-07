namespace DeepSleep
{
    using DeepSleep.Discovery;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// Route '{registration.HttpMethod} {registration.Template}' already has been added.
        /// or
        /// Controller must be specified
        /// or
        /// </exception>
        /// <exception cref="MissingMethodException"></exception>
        public virtual IApiRoutingTable AddRoute(ApiRouteRegistration registration)
        {
            if (registration == null)
            {
                return this;
            }

            if (registration?.Controller == null)
            {
                throw new Exception("Controller must be specified");
            }

            if (string.IsNullOrWhiteSpace(registration.Endpoint))
            {
                throw new Exception("Endpoint must be specified");
            }

            if (registration.HttpMethods?.Count <= 0)
            {
                throw new Exception(string.Format("Http methods not specified on {1}:{0}", registration.Endpoint, registration.Controller.FullName));
            }

            foreach (var httpMethod in registration.HttpMethods)
            {
                var existing = this.routes
                    .Where(r => string.Equals(r.Template, registration.Template, StringComparison.OrdinalIgnoreCase))
                    .Where(r => string.Equals(r.HttpMethod, httpMethod, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (existing != null)
                {
                    throw new Exception($"Route '{httpMethod} {registration.Template}' already has been added.");
                }

                var method = registration.Controller.GetMethod(
                    name: registration.Endpoint,
                    bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

                if (method == null)
                {
                    throw new MissingMethodException(string.Format("Endpoint '{0}' does not exist on controller '{1}'", registration.Endpoint, registration.Controller.FullName));
                }

                var item = new ApiRoutingItem
                {
                    Template = registration.Template,
                    HttpMethod = httpMethod.ToUpperInvariant(),
                    Configuration = registration.Configuration,
                    Location = new ApiEndpointLocation(
                        controller: Type.GetType(registration.Controller.AssemblyQualifiedName),
                        endpoint: registration.Endpoint,
                        httpMethod: httpMethod.ToUpperInvariant())
                };

                routes.Add(item);
            }

            return this;
        }

        /// <summary>Adds the routes.</summary>
        /// <param name="registrations">The registrations.</param>
        /// <returns></returns>
        public virtual IApiRoutingTable AddRoutes(IList<ApiRouteRegistration> registrations)
        {
            if (registrations == null)
            {
                return this;
            }

            registrations.ForEach(r => this.AddRoute(r));

            return this;
        }
    }
}