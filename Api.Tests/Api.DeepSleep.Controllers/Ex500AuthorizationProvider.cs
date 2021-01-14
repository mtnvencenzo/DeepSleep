namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class Ex500AuthorizationProvider : IAuthorizationProvider
    {
        /// <summary>Authorizes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver)
        {
            throw new Exception();
        }
    }
}
