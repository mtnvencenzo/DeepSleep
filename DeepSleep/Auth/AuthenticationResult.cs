namespace DeepSleep.Auth
{
    using System.Collections.Generic;

    /// <summary></summary>
    public class AuthenticationResult
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult" /> class.</summary>
        /// <param name="isAuthenticated">If set to <c>true</c> [is authenticated].</param>
        /// <param name="resource">The resource.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        public AuthenticationResult(bool isAuthenticated, string resource, IApiResponseMessageConverter responseMessageConverter) : this(isAuthenticated)
        {
            this.AddResourceError(resource, responseMessageConverter);
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.</summary>
        /// <param name="isAuthenticated">If set to <c>true</c> [is authenticated].</param>
        public AuthenticationResult(bool isAuthenticated)
            : this()
        {
            IsAuthenticated = isAuthenticated;
        }

        /// <summary>Initializes a new instance of the <see cref="AuthenticationResult"/> class.</summary>
        public AuthenticationResult()
        {
            Errors = new List<ApiResponseMessage>();
        }

        #endregion

        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        public List<ApiResponseMessage> Errors { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is authenticated.</summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        public bool IsAuthenticated { get; set; }

        /// <summary>Gets or sets the authentication token.</summary>
        /// <value>The authentication token.</value>
        public string AuthenticationToken { get; set; }
    }

    /// <summary></summary>
    public static class AuthenticationResultExtensions
    {
        /// <summary>Adds the resource error.</summary>
        /// <param name="result">The result.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns>The <see cref="AuthenticationResult" />.</returns>
        public static AuthenticationResult AddResourceError(this AuthenticationResult result, string resource, IApiResponseMessageConverter responseMessageConverter)
        {
            result.Errors.Add(responseMessageConverter.Convert(resource));
            return result;
        }
    }
}