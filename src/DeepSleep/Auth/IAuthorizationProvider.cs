namespace DeepSleep.Auth
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an authorization provider associated capable of authorizing a request.
    /// </summary>
    public interface IAuthorizationProvider
    {
        /// <summary>Authorizes the request.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="AuthorizationResult"/> detailing the result of the authorization</returns>
        Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver);
    }
}