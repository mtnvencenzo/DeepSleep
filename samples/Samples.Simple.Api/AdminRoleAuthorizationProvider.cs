namespace Samples.Simple.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeepSleep;
    using DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AdminRoleAuthorizationProvider : IAuthorizationProvider
    {
        /// <summary>Authorizes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();
            var rolesNeeded = new List<string> { "AdminUser" };

            var principal = context.Request.ClientAuthenticationInfo.Principal;

            if ((principal?.Identity?.IsAuthenticated ?? false) == false)
            {
                return Task.FromResult(new AuthorizationResult("NotAuthenticated"));
            }

            foreach (var role in rolesNeeded)
            {
                if (!principal.IsInRole(role))
                {
                    return Task.FromResult(new AuthorizationResult("InaccessibleRole"));
                }
            }

            return Task.FromResult(new AuthorizationResult(true));
        }
    }
}
