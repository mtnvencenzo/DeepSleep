namespace DeepSleep.Auth
{
    using System.Threading.Tasks;

    /// <summary></summary>
    public interface IAuthorizationProvider
    {
        /// <summary>Authorizes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        Task<AuthorizationResult> Authorize(ApiRequestContext context);

        /// <summary>Determines whether this instance [can handle authorization policy] the specified scheme.</summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        bool CanHandleAuthPolicy(string policy);

        /// <summary>Gets the scheme.</summary>
        /// <value>The scheme.</value>
        string Policy { get; }
    }
}