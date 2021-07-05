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
    public class ApiRoutingTable : IApiRoutingTable
    {
        private readonly List<ApiRoutingItem> routes;
        private readonly string routePrefix;
        private static string[] methods = new[]
        {
            "GET",
            "HEAD",
            "POST",
            "PUT",
            "DELETE",
            "CONNECT",
            "OPTIONS",
            "TRACE",
            "PATCH"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRoutingTable"/> class.
        /// </summary>
        public ApiRoutingTable(string routePrefix = null)
        {
            routes = new List<ApiRoutingItem>();

            if (!string.IsNullOrWhiteSpace(routePrefix))
            {
                this.routePrefix = routePrefix?.Trim();

                if (this.routePrefix.StartsWith("/") && this.routePrefix.Length == 1)
                {
                    this.routePrefix = null;
                }
                else if (this.routePrefix.StartsWith("/"))
                {
                    this.routePrefix = this.routePrefix.Substring(1);
                }
            }
        }

        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        public virtual IList<ApiRoutingItem> GetRoutes()
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
        public virtual IApiRoutingTable AddRoute(DeepSleepRouteRegistration registration)
        {
            if (registration == null)
            {
                return this;
            }

            if (registration.Controller == null)
            {
                throw new Exception($"{nameof(registration.Controller)} must be specified");
            }

            if (string.IsNullOrWhiteSpace(registration.Endpoint))
            {
                throw new Exception($"{nameof(registration.Endpoint)} must be specified");
            }

            if (registration.HttpMethods?.Where(h => h != null).ToList().Count <= 0)
            {
                throw new Exception(string.Format("Http methods not specified on {1}:{0}", registration.Endpoint, registration.Controller.FullName));
            }

            if (registration.Template == null)
            {
                throw new Exception($"{nameof(registration.Template)} must be specified");
            }

            registration.HttpMethods.Where(i => i != null).ToList().ForEach(m =>
            {
                if (!methods.Contains(m.ToUpper()))
                {
                    throw new Exception($"HTTP Method '{m.ToUpper()} is not supported");
                }
            });

            var template = registration.Template.Trim();

            if (template.StartsWith("/") && template.Length == 1)
            {
                template = "";
            }
            else if (template.StartsWith("/"))
            {
                template = template.Substring(1);
            }

            if (!string.IsNullOrWhiteSpace(this.routePrefix))
            {
                template = (this.routePrefix.EndsWith("/"))
                    ? $"{this.routePrefix}{template}"
                    : $"{this.routePrefix}/{template}";
            }

            foreach (var httpMethod in registration.HttpMethods.Where(h => h != null))
            {
                var existing = this.routes
                    .Where(r => string.Equals(r.Template, template, StringComparison.OrdinalIgnoreCase))
                    .Where(r => string.Equals(r.HttpMethod, httpMethod.Trim(), StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                if (existing != null)
                {
                    throw new Exception($"Route '{httpMethod} {registration.Template}' already has been added.");
                }

                MethodInfo methodInfo = registration.MethodInfo;

                if (methodInfo == null)
                {
                    try
                    {
                        methodInfo = registration.Controller.GetMethod(
                            name: registration.Endpoint.Trim(),
                            bindingAttr: BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        throw new Exception($"DeepSleep api routing does not support routes mapped to overloaded methods for api routes.  You must rename or move the method for route '[{httpMethod}] {template}'.", ex);
                    }
                }

                if (methodInfo == null)
                {
                    throw new MissingMethodException($"Endpoint '{registration.Endpoint}' does not exist on controller '{registration.Controller.FullName}'");
                }

                var item = new ApiRoutingItem
                {
                    Template = template,
                    HttpMethod = httpMethod.ToUpperInvariant().Trim(),
                    Configuration = registration.Configuration,
                    Location = new ApiEndpointLocation(
                        controller: Type.GetType(registration.Controller.AssemblyQualifiedName),
                        methodInfo: methodInfo,
                        httpMethod: httpMethod.ToUpperInvariant().Trim())
                };

                routes.Add(item);
            }

            return this;
        }

        /// <summary>Adds the routes.</summary>
        /// <param name="registrations">The registrations.</param>
        /// <returns></returns>
        public virtual IApiRoutingTable AddRoutes(IList<DeepSleepRouteRegistration> registrations)
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