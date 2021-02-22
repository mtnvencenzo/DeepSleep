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

        /// <summary>Gets or sets the HTTP status mode.</summary>
        /// <value>The HTTP status mode.</value>
        public ValidationHttpStatusMode HttpStatusMode { get; set; }

        /// <summary>Gets the URI binding error status code.</summary>
        /// <value>The URI binding error status code.</value>
        public int UriBindingErrorStatusCode
        {
            get
            {
                if (HttpStatusMode == ValidationHttpStatusMode.StrictHttpSpecification)
                {
                    return 404;
                }

                return 400;
            }
        }

        /// <summary>Gets the body validation error status code.</summary>
        /// <value>The body validation error status code.</value>
        public int BodyValidationErrorStatusCode
        {
            get
            {
                if (HttpStatusMode == ValidationHttpStatusMode.StrictHttpSpecification)
                {
                    return 422;
                }

                return 400;
            }
        }
    }
}
