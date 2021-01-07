namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("[{HttpMethod}] {Template}")]
    public class ApiRouteRegistration
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteRegistration"/> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        public ApiRouteRegistration(string template, IList<string> httpMethods, Type controller, string endpoint)
            : this(template, httpMethods, controller, endpoint, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteRegistration"/> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="System.Exception">
        /// Controller must be specified
        /// or
        /// Endpoint must be specified
        /// or
        /// </exception>
        /// <exception cref="System.MissingMethodException"></exception>
        public ApiRouteRegistration(string template, IList<string> httpMethods, Type controller, string endpoint, IApiRequestConfiguration config)
        {
            if (controller == null)
            {
                throw new Exception("Controller must be specified");
            }

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new Exception("Endpoint must be specified");
            }

            if (httpMethods?.Count <= 0)
            {
                throw new Exception(string.Format("Http methods not specified", endpoint, controller.FullName));
            }

            this.Template = template ?? string.Empty;
            this.HttpMethods = httpMethods.Select(m => m.ToUpperInvariant()).ToList();
            this.Configuration = config;
            this.Controller = Type.GetType(controller.AssemblyQualifiedName);
            this.Endpoint = endpoint;
        }

        /// <summary>Gets or sets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; }

        /// <summary>Gets the HTTP methods.</summary>
        /// <value>The HTTP methods.</value>
        public IList<string> HttpMethods { get; }

        /// <summary>Gets or sets the controller.</summary>
        /// <value>The controller.</value>
        [JsonIgnore]
        public Type Controller { get; set; }

        /// <summary>Gets or sets the endpoint.</summary>
        /// <value>The endpoint.</value>
        public string Endpoint { get; set; }

        /// <summary>Gets or sets the configuration.</summary>
        /// <value>The configuration.</value>
        public IApiRequestConfiguration Configuration { get; }
    }
}
