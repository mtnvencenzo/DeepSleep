namespace DeepSleep
{
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;

    /// <summary>The API processing info.</summary>
    public class ApiProcessingInfo
    {
        /// <summary>Initializes a new instance of the <see cref="ApiProcessingInfo"/> class.</summary>
        public ApiProcessingInfo()
        {
            Exceptions = new List<Exception>();
            UTCRequestDuration = new ApiRequestDuration();
            Validation = new ApiValidationInfo();
        }

        /// <summary>Gets or sets the exception.</summary>
        /// <value>The exception.</value>
        public List<Exception> Exceptions { get; set; }

        /// <summary>Gets or sets the duration of the request.</summary>
        /// <value>The duration of the request.</value>
        public ApiRequestDuration UTCRequestDuration { get; set; }

        /// <summary>Gets or sets the validation.</summary>
        /// <value>The validation.</value>
        public ApiValidationInfo Validation { get; set; }
    }
}