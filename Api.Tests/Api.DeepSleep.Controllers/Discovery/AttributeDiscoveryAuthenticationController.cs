namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryAuthenticationController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/allowanonymous/true")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        public AttributeDiscoveryModel GetAuthenticationAnonymousTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/allowanonymous/false")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationAnonymousFalse()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/notspecified")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationSchemesNotSpecified()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/specified/empty")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationSchemesSpecifiedEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/specified")]
        [ApiAuthentication(authenticationProviderType: typeof(StaticTokenAuthenticationProvider))]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationSchemesSpecified()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
