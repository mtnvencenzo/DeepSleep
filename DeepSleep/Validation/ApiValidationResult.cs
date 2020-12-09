using System;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>Gets or sets the suggested HTTP status code.</summary>
        /// <value>The suggested HTTP status code.</value>
        public int SuggestedHttpStatusCode { get; set; } = 400;

        /// <summary>Singles the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static List<ApiValidationResult> Single(string message)
        {
            return new List<ApiValidationResult>
            {
                new ApiValidationResult
                {
                    IsValid = false,
                    Message = message
                }
            };
        }

        /// <summary>Singles the specified message.</summary>
        /// <param name="message">The message.</param>
        /// <param name="suggestedHttpStatusCode">The suggested HTTP status code.</param>
        /// <returns></returns>
        public static List<ApiValidationResult> Single(string message, int suggestedHttpStatusCode)
        {
            return new List<ApiValidationResult>
            {
                new ApiValidationResult
                {
                    IsValid = false,
                    Message = message,
                    SuggestedHttpStatusCode = suggestedHttpStatusCode
                }
            };
        }

        /// <summary>Singles the specified message.</summary>
        /// <returns></returns>
        public static List<ApiValidationResult> Multiple(IEnumerable<string> messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            return new List<ApiValidationResult>(messages.Select(m => new ApiValidationResult(m)));
        }
    }
}
