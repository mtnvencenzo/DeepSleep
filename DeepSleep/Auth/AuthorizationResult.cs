namespace DeepSleep.Auth
{
    using System.Collections.Generic;

    /// <summary>
    /// The authorization result provided by an <see cref="DeepSleep.Auth.IAuthorizationProvider"/>.
    /// </summary>
    public class AuthorizationResult
    {
        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult" /> class setting the <see cref="IsAuthorized"/> flag to <c>false</c>.</summary>
        /// <param name="errors">The errors detailing why the request was not authorized.</param>
        public AuthorizationResult(params string[] errors) : this(false)
        {
            if (errors != null)
            {
                errors.ForEach(error => this.AddResourceError(error));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult"/> class.</summary>
        /// <param name="isAuthorized">If set to <c>true</c> [is authorized], otherwise <c>false</c> [not authorized].</param>
        public AuthorizationResult(bool isAuthorized)
            : this()
        {
            IsAuthorized = isAuthorized;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult"/> class.
        /// </summary>
        internal AuthorizationResult()
        {
            Errors = new List<string>();
        }

        /// <summary>Gets the errors associated with the un-authorized request.</summary>
        /// <value>The errors associated with the un-authorized request.</value>
        public List<string> Errors { get; private set; }

        /// <summary>Gets a value indicating whether this instance is authorized.</summary>
        /// <value><c>true</c> if this instance is authorized; otherwise, <c>false</c>.</value>
        public bool IsAuthorized { get; private set; }
    }

    /// <summary>
    /// Extension methods for the <see cref="AuthorizationResult" />
    /// </summary>
    public static class AuthorizationResultExtensions
    {
        /// <summary>Adds an error to the <see cref="AuthorizationResult"/>.</summary>
        /// <param name="result">The authorization result.</param>
        /// <param name="error">The error to add to the <see cref="AuthorizationResult"/>.</param>
        /// <remarks>
        /// If the error is null or whitespace it will not be added.
        /// </remarks>
        /// <returns>The <see cref="AuthorizationResult" />.</returns>
        public static AuthorizationResult AddResourceError(this AuthorizationResult result, string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                result.Errors.Add(error);
            }

            return result;
        }
    }
}