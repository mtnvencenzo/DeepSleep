namespace DeepSleep.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationErrorConfiguration
    {
        /// <summary>Gets or sets the URI binding error.</summary>
        /// <value>The URI binding error.</value>
        public virtual string UriBindingError { get; set; }

        /// <summary>Gets or sets the URI binding value error.</summary>
        /// <value>The URI binding value error.</value>
        public virtual string UriBindingValueError { get; set; }

        /// <summary>Gets or sets the request deserialization error.</summary>
        /// <value>The request deserialization error.</value>
        public virtual string RequestDeserializationError { get; set; }

        /// <summary>Gets or sets the use custom status for request deserialization errors.</summary>
        /// <value>The use custom status for request deserialization errors.</value>
        public bool? UseCustomStatusForRequestDeserializationErrors { get; set; }
    }
}
