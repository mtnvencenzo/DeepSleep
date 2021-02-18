namespace DeepSleep.Validation
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class ApiValidationResult
    {
        /// <summary>Initializes a new instance of the <see cref="ApiValidationResult"/> class.</summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        /// <param name="message">The message.</param>
        /// <param name="suggestedHttpStatusCode">The suggested HTTP status code.</param>
        internal ApiValidationResult(bool isValid, string message = null, int? suggestedHttpStatusCode = null)
        {
            this.IsValid = isValid;
            this.Message = message;
            this.SuggestedHttpStatusCode = suggestedHttpStatusCode;
        }

        /// <summary>Returns true if ... is valid.</summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid { get; }

        /// <summary>Gets the message.</summary>
        /// <value>The message.</value>
        public string Message { get; }

        /// <summary>Gets or sets the suggested HTTP status code.</summary>
        /// <value>The suggested HTTP status code.</value>
        public int? SuggestedHttpStatusCode { get; }

        /// <summary>Successes this instance.</summary>
        /// <returns></returns>
        public static IList<ApiValidationResult> Success()
        {
            return new List<ApiValidationResult>
            {
                new ApiValidationResult(
                    isValid: true,
                    message: null,
                    suggestedHttpStatusCode: null)
            };
        }

        /// <summary>Successes this instance.</summary>
        /// <returns></returns>
        public static ApiValidationResult Failure(string message = null, int? suggestedHttpStatusCode = null)
        {
            return new ApiValidationResult(
                isValid: false,
                message: message,
                suggestedHttpStatusCode: suggestedHttpStatusCode);
        }

        /// <summary>Singles the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="suggestedHttpStatusCode">The suggested HTTP status code.</param>
        /// <returns></returns>
        public static IList<ApiValidationResult> Single(string message, int? suggestedHttpStatusCode = null)
        {
            return new List<ApiValidationResult>
            {
                new ApiValidationResult(
                    isValid: false,
                    message: message,
                    suggestedHttpStatusCode: suggestedHttpStatusCode)
            };
        }

        /// <summary>Multiples the specified messages.</summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static IList<ApiValidationResult> Multiple(IEnumerable<string> messages)
        {
            if (messages == null)
            {
                return new List<ApiValidationResult>();
            }

            return new List<ApiValidationResult>(messages.Select(m => new ApiValidationResult(isValid: false, message: m)));
        }
    }
}
