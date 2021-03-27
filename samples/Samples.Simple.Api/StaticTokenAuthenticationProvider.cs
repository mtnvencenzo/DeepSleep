namespace Samples.Simple.Api
{
    using System;
    using System.Threading.Tasks;
    using DeepSleep;
    using DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class StaticTokenAuthenticationProvider : IAuthenticationProvider
    {
        public string Realm => "DeepSleepSamples";
        public string Scheme => "Token";

        public Task<AuthenticationResult> Authenticate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();
            var clientValue = context.Request.ClientAuthenticationInfo.AuthValue ?? string.Empty;
            var isMatch = clientValue == "91E4BCD0-9C67-4AA1-9821-D2AC6B7AB037";

            if (!isMatch)
            {
                return Task.FromResult(new AuthenticationResult("InvalidToken"));
            }

            var myIPrincipal = new SecurityContextPrincipal(1, new[] { "SiteUser" }, "AuthType");

            return Task.FromResult(new AuthenticationResult(true, myIPrincipal));
        }

        public bool CanHandleAuthScheme(string scheme)
        {
            if (string.IsNullOrWhiteSpace(scheme))
                return false;

            if (scheme.Equals(this.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}
