namespace DeepSleep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteAttribute"/> class.</summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="template">The template.</param>
        public ApiRouteAttribute(string httpMethod, string template)
            : this(httpMethods: new[] { httpMethod }, template: template, deprecated: false)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteAttribute"/> class.</summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="template">The template.</param>
        /// <param name="deprecated">if set to <c>true</c> [deprecated].</param>
        public ApiRouteAttribute(string httpMethod, string template, bool deprecated)
            : this(httpMethods: new[] { httpMethod }, template: template, deprecated: deprecated)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteAttribute"/> class.</summary>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="template">The template.</param>
        /// <exception cref="System.ArgumentException">
        /// httpMethod
        /// or
        /// template
        /// </exception>
        public ApiRouteAttribute(string[] httpMethods, string template)
            : this(httpMethods: httpMethods, template: template, deprecated: false)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteAttribute"/> class.</summary>
        /// <param name="httpMethods">The HTTP methods.</param>
        /// <param name="template">The template.</param>
        /// <param name="deprecated">if set to <c>true</c> [deprecated].</param>
        /// <exception cref="System.ArgumentException">
        /// httpMethod
        /// or
        /// template
        /// </exception>
        public ApiRouteAttribute(string[] httpMethods, string template, bool deprecated)
        {
            var methods = httpMethods
                ?.Where(h => !string.IsNullOrWhiteSpace(h))
                ?.ToList();

            if ((methods?.Count ?? 0) <= 0)
            {
                throw new ArgumentException($"{nameof(httpMethods)} must be specified", nameof(httpMethods));
            }

            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentException($"{nameof(template)} must be specified", nameof(template));
            }

            this.HttpMethods = methods;
            this.Template = template;
            this.Deprecated = deprecated;
        }

        /// <summary>Gets the HTTP methods.</summary>
        /// <value>The HTTP methods.</value>
        public IList<string> HttpMethods { get; private set; }

        /// <summary>Gets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; private set; }

        /// <summary>Gets the deprecated.</summary>
        /// <value>The deprecated.</value>
        public bool? Deprecated { get; private set; }
    }
}
