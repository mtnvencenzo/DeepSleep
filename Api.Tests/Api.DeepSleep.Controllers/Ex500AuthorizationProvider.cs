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
        /// <summary>
        /// 
        /// </summary>
        public string Policy => "EX-500";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<AuthorizationResult> Authorize(ApiRequestContext context)
        {
            throw new Exception();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public bool CanHandleAuthPolicy(string policy)
        {
            if (string.IsNullOrWhiteSpace(policy))
                return false;

            if (policy.Equals(this.Policy, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
