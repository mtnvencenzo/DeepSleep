namespace DeepSleep.Auth
{
    using System.Collections.Generic;
    using System.Security.Principal;

    /// <summary>
    /// The authentication result provided by an <see cref="DeepSleep.Auth.IAuthorizationProvider"/>.
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult" /> class setting the <see cref="IsAuthenticated"/> flag to <c>false</c>.</summary>
        /// <param name="errors">The errors detailing why the request was not authenticated.</param>
        public AuthenticationResult(params string[] errors) : this(false)
        {
            if (errors != null)
            {
                errors.ForEach(error => this.AddResourceError(error));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.</summary>
        /// <param name="isAuthenticated">If set to <c>true</c> [is authenticated], otherwise <c>false</c> [not authenticated].</param>
        public AuthenticationResult(bool isAuthenticated)
            : this()
        {
            IsAuthenticated = isAuthenticated;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.</summary>
        /// <param name="isAuthenticated">If set to <c>true</c> [is authenticated], otherwise <c>false</c> [not authenticated].</param>
        /// <param name="principal">The security principal determined during authentication.</param>
        public AuthenticationResult(bool isAuthenticated, IPrincipal principal)
            : this()
        {
            IsAuthenticated = isAuthenticated;
            Principal = principal;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.
        /// </summary>
        internal AuthenticationResult()
        {
            Errors = new List<string>();
        }

        /// <summary>Gets the errors associated with the un-authenticated request.</summary>
        /// <value>The errors associated with the un-authenticated request.</value>
        public List<string> Errors { get; private set; }

        /// <summary>Gets a value indicating whether this instance is authenticated.</summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated { get; private set; }

        /// <summary>Gets the security principal determined during authentication.</summary>
        /// <value>The security principal associated with the authenticated request.</value>
        public IPrincipal Principal { get; private set; }
    }

    /// <summary>
    /// Extension methods for the <see cref="AuthenticationResult" />
    /// </summary>
    public static class AuthenticationResultExtensions
    {
        /// <summary>Adds an error to the <see cref="AuthenticationResult"/>.</summary>
        /// <param name="result">The result.</param>
        /// <param name="error">The error to add to the <see cref="AuthenticationResult"/>.</param>
        /// <remarks>
        /// If the error is null or whitespace it will not be added.
        /// </remarks>
        /// <returns>The <see cref="AuthenticationResult" />.</returns>
        public static AuthenticationResult AddResourceError(this AuthenticationResult result, string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                result.Errors.Add(error);
            }

            return result;
        }
    }
}