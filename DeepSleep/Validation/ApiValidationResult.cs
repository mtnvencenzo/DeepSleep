namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationResult
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="ApiValidationResult"/> class.</summary>
        public ApiValidationResult()
        {
            IsValid = false;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiValidationResult"/> class.</summary>
        /// <param name="message">The message.</param>
        public ApiValidationResult(ApiResponseMessage message) : this()
        {
            Message = message;
        }

        #endregion

        /// <summary>Gets or sets a value indicating whether this instance is success.</summary>
        /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
        public bool IsValid { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public ApiResponseMessage Message { get; set; }

        /// <summary>Singles the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static List<ApiValidationResult> Single(ApiResponseMessage message)
        {
            return new List<ApiValidationResult>
            {
                new ApiValidationResult(message)
            };
        }
    }
}
