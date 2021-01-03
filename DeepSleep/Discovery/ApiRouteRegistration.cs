namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("[{HttpMethod}] {Template}")]
    public class ApiRouteRegistration
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteRegistration"/> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        public ApiRouteRegistration(string template, string httpMethod, Type controller, string endpoint)
            : this(template, httpMethod, controller, endpoint, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteRegistration"/> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="Exception">
        /// Controller must be specified
        /// or
        /// Endpoint must be specified
        /// or
        /// Template must be specified
        /// or
        /// </exception>
        /// <exception cref="MissingMethodException"></exception>
        public ApiRouteRegistration(string template, string httpMethod, Type controller, string endpoint, IApiRequestConfiguration config)
        {
            if (controller == null)
            {
                throw new Exception("Controller must be specified");
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new Exception("Endpoint must be specified");
            }

            if (string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new Exception(string.Format("Http method not specified", endpoint, controller.FullName));
            }

            var method = controller.GetMethod(endpoint, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            if (method == null)
            {
                throw new MissingMethodException(string.Format("Endpoint '{0}' does not exist on controller '{1}'", endpoint, controller.FullName));
            }

            this.Template = template ?? string.Empty;
            this.HttpMethod = httpMethod.ToUpperInvariant();
            this.Configuration = config;
            this.Location = new ApiEndpointLocation
            {
                Controller = Type.GetType(controller.AssemblyQualifiedName),
                Endpoint = endpoint,
                HttpMethod = httpMethod.ToUpperInvariant()
            };
        }

        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; }

        /// <summary>Gets or sets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; }

        /// <summary>Gets or sets the endpoint location.</summary>
        /// <value>The endpoint location.</value>
        public ApiEndpointLocation Location { get; }

        /// <summary>Gets or sets the configuration.</summary>
        /// <value>The configuration.</value>
        public IApiRequestConfiguration Configuration { get; }
    }
}
