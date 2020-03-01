namespace DeepSleep.Auth
{
    using System.Threading.Tasks;

    /// <summary></summary>
    public interface IAuthenticationProvider
    {
        /// <summary>Authenticates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns>The <see cref="Task" />.</returns>
        Task Authenticate(ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter);

        /// <summary>Determines whether this instance [can handle authentication scheme] the specified scheme.</summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        bool CanHandleAuthScheme(string scheme);

        /// <summary>Gets the realm.</summary>
        /// <value>The realm.</value>
        string Realm { get; }

        /// <summary>Gets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        string Scheme { get; }
    }
}