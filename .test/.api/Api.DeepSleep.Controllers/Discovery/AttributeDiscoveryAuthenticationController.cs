namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryAuthenticationController
    {
        /// <summary>Gets the authentication anonymous true.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/allowanonymous/true")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        public AttributeDiscoveryModel GetAuthenticationAnonymousTrue()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authentication anonymous false.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/allowanonymous/false")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationAnonymousFalse()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authentication schemes not specified.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/notspecified")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationSchemesNotSpecified()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authentication schemes specified empty.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/specified/empty")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationSchemesSpecifiedEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authentication schemes specified.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/specified")]
        [ApiAuthentication(authenticationProviderType: typeof(StaticTokenAuthenticationProvider))]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationSchemesSpecified()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
