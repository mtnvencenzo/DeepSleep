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
            var token = context.Request.ClientAuthenticationInfo.AuthValue ?? string.Empty;

            var match = token == "token-oasoasjjuq09qrufaisfasaasjd";

            if (!match)
            {
                return Task.FromResult(new AuthenticationResult("Invalid Token"));
            }

            return Task.FromResult(new AuthenticationResult(true));
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
