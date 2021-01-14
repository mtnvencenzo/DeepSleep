namespace DeepSleep.Tests
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultAuthorizationProvider : IAuthorizationProvider
    {
        /// <summary>Authorizes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver)
        {
            return Task.FromResult(new AuthorizationResult(true));
        }
    }
}
