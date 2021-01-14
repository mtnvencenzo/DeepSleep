namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class Ex501AuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>Gets the realm.</summary>
        /// <value>The realm.</value>
        public string Realm => "Api-Unit-Test";

        /// <summary>Gets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public string Scheme => "EX-501";

        /// <summary>Authenticates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        /// <exception cref="ApiNotImplementedException"></exception>
        public Task<AuthenticationResult> Authenticate(IApiRequestContextResolver contextResolver)
        {
            throw new ApiNotImplementedException();
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
