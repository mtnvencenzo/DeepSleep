﻿namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class StaticTokenAuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>Gets the realm.</summary>
        /// <value>The realm.</value>
        public string Realm => "Api-Unit-Test";

        /// <summary>Gets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public string Scheme => "Token";

        /// <summary>Authenticates the specified context.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" />.</returns>
        /// <exception cref="ArgumentException">StaticToken.Token is null or empty</exception>
        public Task<AuthenticationResult> Authenticate(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver?.GetContext();
            var acceptable = "T0RrMlJqWXpNVFF0UmtReFF5MDBRamN5TFVJeE5qZ3RPVGxGTlRBek5URXdNVUkz";
            var authValue = context.Request.ClientAuthenticationInfo.AuthValue ?? string.Empty;

            if (string.IsNullOrWhiteSpace(authValue))
            {
                return Task.FromResult(new AuthenticationResult("EmptyStaticToken"));
            }

            if (authValue != acceptable)
            {
                return Task.FromResult(new AuthenticationResult("InvalidStaticToken"));
            }

            return Task.FromResult(new AuthenticationResult(true, null as IPrincipal));
        }

        /// <summary>Determines whether this instance [can handle authentication scheme] the specified scheme.</summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
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
