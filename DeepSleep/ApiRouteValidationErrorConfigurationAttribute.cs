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
        /// <param name="requestDeserializationError">The request deserialization error.</param>
        /// <param name="useCustomStatusForRequestDeserializationErrors">if set to <c>true</c> [use custom status for request deserialization errors].</param>
        public ApiRouteValidationErrorConfigurationAttribute(
            string uriBindingError = null, 
            string uriBindingValueError = null, 
            string requestDeserializationError = null,
            bool useCustomStatusForRequestDeserializationErrors = true)
        {
            this.UriBindingError = uriBindingError;
            this.UriBindingValueError = uriBindingValueError;
            this.RequestDeserializationError = requestDeserializationError;
            this.UseCustomStatusForRequestDeserializationErrors = useCustomStatusForRequestDeserializationErrors;
        }

        /// <summary>Gets the URI binding error.</summary>
        /// <value>The URI binding error.</value>
        public string UriBindingError { get; private set; }

        /// <summary>Gets the URI binding value error.</summary>
        /// <value>The URI binding value error.</value>
        public string UriBindingValueError { get; private set; }

        /// <summary>Gets the request deserialization error.</summary>
        /// <value>The request deserialization error.</value>
        public string RequestDeserializationError { get; private set; }

        /// <summary>Gets the use custom status for request deserialization errors.</summary>
        /// <value>The use custom status for request deserialization errors.</value>
        public bool? UseCustomStatusForRequestDeserializationErrors { get; private set; }
    }
}
