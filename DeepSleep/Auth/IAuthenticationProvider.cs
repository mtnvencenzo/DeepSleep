namespace DeepSleep.Auth
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an authentication provider associated to an Authorization header scheme capable of authenticating a request.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>Authenticates the request.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="AuthenticationResult"/> detailing the result of the authentication.</returns>
        Task<AuthenticationResult> Authenticate(IApiRequestContextResolver contextResolver);

        /// <summary>Determines if this <see cref="IAuthenticationProvider"/> is capable of authentating the request based on the requests Authorization header scheme.</summary>
        /// <param name="scheme">The Authorization header that scheme this <see cref="IAuthenticationProvider"/> is capable of authenticating. (Example) Bearer</param>
        /// <returns><c>true</c> if this instance can authentcate against the provided Auorization header scheme; otherwise, <c>false</c>.</returns>
        bool CanHandleAuthScheme(string scheme);

        /// <summary>Gets the protected area realm that this provider is associated with.</summary>
        /// <remarks>
        /// Used within a WWW-Authenticate response header when generating a 401 Unauthorized response. (Example) WWW-Authenticate Bearer realm="My Realm"
        /// <para>
        /// [see-also] Hypertext Transfer Protocol (14.47 WWW-Authenticate: <see href="https://tools.ietf.org/html/rfc2616#section-14.47"/>)
        /// </para>
        /// </remarks>
        /// <value>The protected area realm.</value>
        string Realm { get; }

        /// <summary>The Authorization header scheme that this <see cref="IAuthenticationProvider"/> is capable of authenticating. (Example) Bearer</summary>
        /// <remarks>
        /// [see-also] Hypertext Transfer Protocol (4.2.  Authorization: <see href="https://tools.ietf.org/html/rfc7235#section-4.2"/>)
        /// </remarks>
        /// <value>The Authorization header scheme.</value>
        string Scheme { get; }
    }
}