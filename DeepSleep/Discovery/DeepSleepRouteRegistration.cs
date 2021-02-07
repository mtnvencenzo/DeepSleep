namespace DeepSleep.Discovery
{
    using DeepSleep.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("[{HttpMethods}] {Template}")]
    public class DeepSleepRouteRegistration
    {
        /// <summary>Initializes a new instance of the <see cref="DeepSleepRouteRegistration"/> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="endpoint">The endpoint.</param>
        public DeepSleepRouteRegistration(
            string template, 
            IList<string> httpMethods, 
            Type controller, 
            string endpoint)
            : this(template, httpMethods, controller, endpoint, null, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DeepSleepRouteRegistration" /> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="methodInfo">The method information.</param>
        public DeepSleepRouteRegistration(
            string template,
            IList<string> httpMethods,
            Type controller,
            MethodInfo methodInfo)
            : this(template, httpMethods, controller, methodInfo?.Name, methodInfo, null)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DeepSleepRouteRegistration" /> class.</summary>
        /// <param name="template">The template.</param>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="config">The configuration.</param>
        /// <exception cref="Exception">Controller must be specified
        /// or
        /// Endpoint must be specified
        /// or</exception>
        public DeepSleepRouteRegistration(
            string template,
            IList<string> httpMethods,
            Type controller,
            MethodInfo methodInfo,
            IDeepSleepRequestConfiguration config)
            : this(template, httpMethods, controller, methodInfo?.Name, methodInfo, config)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DeepSleepRouteRegistration"/> class.</summary>
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
        public DeepSleepRouteRegistration(
            string template, 
            IList<string> httpMethods, 
            Type controller, 
            string endpoint, 
            IDeepSleepRequestConfiguration config)
            : this(template, httpMethods, controller, endpoint, null, config)
        {
        }

        private DeepSleepRouteRegistration(
            string template,
            IList<string> httpMethods,
            Type controller,
            string endpoint,
            MethodInfo methodInfo,
            IDeepSleepRequestConfiguration config)
        {
            if (controller == null)
            {
                throw new Exception("Controller must be specified");
            }

            if (string.IsNullOrWhiteSpace(endpoint) && methodInfo == null)
            {
                throw new Exception($"{nameof(endpoint)} or {nameof(methodInfo)} must be specified");
            }

            if (httpMethods?.Count == 0)
            {
                throw new Exception(string.Format("Http methods not specified", endpoint, controller.FullName));
            }

            this.Template = template ?? string.Empty;
            this.HttpMethods = httpMethods.Select(m => m.ToUpperInvariant()).ToList();
            this.Configuration = config;
            this.Controller = Type.GetType(controller.AssemblyQualifiedName);
            this.Endpoint = endpoint;

            if (methodInfo != null)
            {
                var parameters = methodInfo.GetParameters();

                var methods = this.Controller
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod)
                    .Where(m => m.Name == methodInfo.Name)
                    .ToList();

                if (methods.Count == 0)
                {
                    this.MethodInfo = null;
                }
                else if (methods.Count == 1)
                {
                    this.MethodInfo = methods[0];
                }
                else
                {
                    // TODO: Handle this for overloads
                }
            }

            this.MethodInfo = methodInfo;
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
        public Type Controller { get; }

        /// <summary>Gets or sets the endpoint.</summary>
        /// <value>The endpoint.</value>
        public string Endpoint { get; }

        /// <summary>Gets or sets the method.</summary>
        /// <value>The method.</value>
        public MethodInfo MethodInfo { get; }

        /// <summary>Gets or sets the configuration.</summary>
        /// <value>The configuration.</value>
        public IDeepSleepRequestConfiguration Configuration { get; }
    }
}
