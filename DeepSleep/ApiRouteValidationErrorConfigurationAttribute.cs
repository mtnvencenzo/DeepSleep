namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ApiRouteValidationErrorConfigurationAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="ApiRouteValidationErrorConfigurationAttribute"/> class.</summary>
        /// <param name="uriBindingError">The URI binding error.</param>
        /// <param name="uriBindingValueError">The URI binding value error.</param>
        public ApiRouteValidationErrorConfigurationAttribute(string uriBindingError = null, string uriBindingValueError = null)
        {
            this.UriBindingError = uriBindingError;
            this.UriBindingValueError = uriBindingValueError;
        }

        /// <summary>Gets the URI binding error.</summary>
        /// <value>The URI binding error.</value>
        public string UriBindingError { get; private set; }

        /// <summary>Gets the URI binding value error.</summary>
        /// <value>The URI binding value error.</value>
        public string UriBindingValueError { get; private set; }
    }
}
