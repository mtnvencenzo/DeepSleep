namespace DeepSleep.Tests
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class Default2AuthorizationProvider : IAuthorizationProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public string Policy => "Default2";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<AuthorizationResult> Authorize(ApiRequestContext context)
        {
            return Task.FromResult(new AuthorizationResult(true));
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
