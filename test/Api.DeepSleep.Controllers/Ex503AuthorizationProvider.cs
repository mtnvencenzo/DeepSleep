namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class Ex503AuthorizationProvider : IAuthorizationProvider
    {
        /// <summary>Authorizes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        /// <exception cref="ApiServiceUnavailableException"></exception>
        public Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver)
        {
            throw new ApiServiceUnavailableException();
        }
    }
}
