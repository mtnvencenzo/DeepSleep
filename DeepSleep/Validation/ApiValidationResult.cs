namespace DeepSleep.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationResult
    {
        /// <summary>Initializes a new instance of the <see cref="ApiValidationResult"/> class.</summary>
        public ApiValidationResult()
        {
            IsValid = false;
        }

        /// <summary>Initializes a new instance of the <see cref="ApiValidationResult"/> class.</summary>
        /// <param name="message">The message.</param>
        public ApiValidationResult(string message) : this()
        {
            Message = message;
        }

        /// <summary>Gets or sets a value indicating whether this instance is success.</summary>
        /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
        public bool IsValid { get; set; }

        /// <summary>Gets or sets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}
