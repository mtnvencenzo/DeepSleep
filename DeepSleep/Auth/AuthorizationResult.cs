namespace DeepSleep.Auth
{
    using System.Collections.Generic;

    /// <summary></summary>
    public class AuthorizationResult
    {
        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult" /> class.</summary>
        /// <param name="isAuthorized">If set to <c>true</c> [is authorized].</param>
        /// <param name="error">The error.</param>
        public AuthorizationResult(bool isAuthorized, string error) : this(isAuthorized)
        {
            this.AddResourceError(error);
        }

        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult"/> class.</summary>
        /// <param name="isAuthorized">If set to <c>true</c> [is authorized].</param>
        public AuthorizationResult(bool isAuthorized)
            : this()
        {
            IsAuthorized = isAuthorized;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult"/> class.
        /// </summary>
        public AuthorizationResult()
        {
            Errors = new List<string>();
        }

        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        public List<string> Errors { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is authorized.</summary>
        /// <value><c>true</c> if this instance is authorized; otherwise, <c>false</c>.</value>
        public bool IsAuthorized { get; }
    }

    /// <summary></summary>
    public static class AuthorizationResultExtensions
    {
        /// <summary>Adds the resource error.</summary>
        /// <param name="result">The result.</param>
        /// <param name="error">The resource.</param>
        /// <returns>The <see cref="AuthorizationResult" />.</returns>
        public static AuthorizationResult AddResourceError(this AuthorizationResult result, string error)
        {
            result.Errors.Add(error);
            return result;
        }
    }
}