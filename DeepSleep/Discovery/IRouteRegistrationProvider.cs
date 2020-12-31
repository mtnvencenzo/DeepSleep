namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IRouteRegistrationProvider
    {
        /// <summary>Gets the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        Task<IList<ApiRouteRegistration>> GetRoutes(IServiceProvider serviceProvider);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class IRouteRegistrationProviderExtensionMethods
    {
        /// <summary>Adds the route.</summary>
        /// <param name="registrations">The registrations.</param>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns></returns>
        public static IList<ApiRouteRegistration> AddRoute(this IList<ApiRouteRegistration> registrations, string template, string httpMethod, Type controller, string endpoint)
        {
            return registrations.AddRoute(template, httpMethod, controller, endpoint, null);
        }

        /// <summary>Adds the route.</summary>
        /// <param name="registrations">The registrations.</param>
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
        public static IList<ApiRouteRegistration> AddRoute(this IList<ApiRouteRegistration> registrations, string template, string httpMethod, Type controller, string endpoint, IApiRequestConfiguration config)
        {
            var existing = registrations
                .Where(r => string.Equals(r.Template, template, StringComparison.OrdinalIgnoreCase))
                .Where(r => string.Equals(r.HttpMethod, httpMethod, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if(existing != null)
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
            return registrations;
        }
    }
}
