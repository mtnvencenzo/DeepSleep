namespace DeepSleep.Auth
{
    using System.Threading.Tasks;

    /// <summary></summary>
    public interface IAuthorizationProvider
    {
        /// <summary>Authorizes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver);
    }
}