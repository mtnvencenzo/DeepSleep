namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryAuthenticationController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/allowanonymous/true")]
        [ApiRouteAuthentication(allowAnonymous: true)]
        public AttributeDiscoveryModel GetAuthenticationAnonymousTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/allowanonymous/false")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthenticationAnonymousFalse()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/notspecified")]
        [ApiRouteAuthentication(supportedAuthenticationSchemes: null)]
        public AttributeDiscoveryModel GetAuthenticationSchemesNotSpecified()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/specified/empty")]
        [ApiRouteAuthentication(supportedAuthenticationSchemes: new string[] {})]
        public AttributeDiscoveryModel GetAuthenticationSchemesSpecifiedEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authentication/schemes/specified")]
        [ApiRouteAuthentication(supportedAuthenticationSchemes: new[] { "Token" })]
        public AttributeDiscoveryModel GetAuthenticationSchemesSpecified()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
