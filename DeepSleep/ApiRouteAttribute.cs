namespace DeepSleep
{
    using System;

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
        /// <exception cref="ArgumentException">
        /// httpMethod
        /// or
        /// template
        /// </exception>
        public ApiRouteAttribute(string httpMethod, string template)
        {
            if (string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new ArgumentException($"{nameof(httpMethod)} must be specified", nameof(httpMethod));
            }

            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentException($"{nameof(template)} must be specified", nameof(template));
            }

            this.HttpMethod = httpMethod;
            this.Template = template;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiRouteAttribute"/> class.</summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="template">The template.</param>
        /// <param name="deprecated">if set to <c>true</c> [deprecated].</param>
        /// <exception cref="ArgumentException">
        /// httpMethod
        /// or
        /// template
        /// </exception>
        public ApiRouteAttribute(string httpMethod, string template, bool deprecated)
        {
            if (string.IsNullOrWhiteSpace(httpMethod))
            {
                throw new ArgumentException($"{nameof(httpMethod)} must be specified", nameof(httpMethod));
            }

            if (string.IsNullOrWhiteSpace(template))
            {
                throw new ArgumentException($"{nameof(template)} must be specified", nameof(template));
            }

            this.HttpMethod = httpMethod;
            this.Template = template;
            this.Deprecated = deprecated;
        }

        /// <summary>Gets the HTTP method.</summary>
        /// <value>The HTTP method.</value>
        public string HttpMethod { get; private set; }

        /// <summary>Gets the template.</summary>
        /// <value>The template.</value>
        public string Template { get; private set; }

        /// <summary>Gets the deprecated.</summary>
        /// <value>The deprecated.</value>
        public bool? Deprecated { get; private set; }
    }
}
