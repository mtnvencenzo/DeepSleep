namespace DeepSleep.Auth
{
    using System.Collections.Generic;

    /// <summary></summary>
    public class AuthorizationResult
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="AuthorizationResult" /> class.</summary>
        /// <param name="isAuthorized">If set to <c>true</c> [is authorized].</param>
        /// <param name="resource">The resource.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        public AuthorizationResult(bool isAuthorized, string resource, IApiResponseMessageConverter responseMessageConverter) : this(isAuthorized)
        {
            this.AddResourceError(resource, responseMessageConverter);
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
            Errors = new List<ApiResponseMessage>();
        }

        #endregion

        /// <summary>Gets or sets the errors.</summary>
        /// <value>The errors.</value>
        public List<ApiResponseMessage> Errors { get; set; }

        /// <summary>Gets or sets a value indicating whether this instance is authorized.</summary>
        /// <value><c>true</c> if this instance is authorized; otherwise, <c>false</c>.</value>
        public bool IsAuthorized { get; set; }
    }

    /// <summary></summary>
    public static class AuthorizationResultExtensions
    {
        /// <summary>Adds the resource error.</summary>
        /// <param name="result">The result.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns>The <see cref="AuthorizationResult" />.</returns>
        public static AuthorizationResult AddResourceError(this AuthorizationResult result, string resource, IApiResponseMessageConverter responseMessageConverter)
        {
            result.Errors.Add(responseMessageConverter.Convert(resource));
            return result;
        }
    }
}