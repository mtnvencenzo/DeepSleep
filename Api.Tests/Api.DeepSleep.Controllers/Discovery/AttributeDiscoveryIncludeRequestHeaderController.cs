namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryIncludeRequestHeaderController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/includeRequestHeader/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteIncludeRequestIdHeader]
        public AttributeDiscoveryModel GetIncludeRequestHeaderDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/includeRequestHeader/true")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteIncludeRequestIdHeader(includeRequestIdHeaderInResponse: true)]
        public AttributeDiscoveryModel GetIncludeRequestHeaderTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/includeRequestHeader/false")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteIncludeRequestIdHeader(includeRequestIdHeaderInResponse: false)]
        public AttributeDiscoveryModel GetIncludeRequestHeaderFalse()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
