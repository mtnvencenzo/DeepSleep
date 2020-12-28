namespace DeepSleep.Auth
{
    using System.Collections.Generic;
    using System.Security.Principal;

    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult" /> class.</summary>
        /// <param name="isAuthenticated">If set to <c>true</c> [is authenticated].</param>
        /// <param name="error">The resource.</param>
        public AuthenticationResult(bool isAuthenticated, string error) : this(isAuthenticated)
        {
            this.AddResourceError(error);
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.</summary>
        /// <param name="isAuthenticated">If set to <c>true</c> [is authenticated].</param>
        public AuthenticationResult(bool isAuthenticated)
            : this()
        {
            IsAuthenticated = isAuthenticated;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.</summary>
        /// <param name="isAuthenticated">if set to <c>true</c> [is authenticated].</param>
        /// <param name="principal">The principal.</param>
        public AuthenticationResult(bool isAuthenticated, IPrincipal principal)
            : this()
        {
            IsAuthenticated = isAuthenticated;
            Principal = principal;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.
        /// </summary>
        public AuthenticationResult()
        {
            Errors = new List<string>();
        }

        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        public List<string> Errors { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is authenticated.</summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated { get; set; }

        /// <summary>Gets or sets the principal.</summary>
        /// <value>The principal.</value>
        public IPrincipal Principal { get; private set; }
    }

    /// <summary></summary>
    public static class AuthenticationResultExtensions
    {
        /// <summary>Adds the resource error.</summary>
        /// <param name="result">The result.</param>
        /// <param name="error">The resource.</param>
        /// <returns>The <see cref="AuthenticationResult" />.</returns>
        public static AuthenticationResult AddResourceError(this AuthenticationResult result, string error)
        {
            result.Errors.Add(error);
            return result;
        }
    }
}