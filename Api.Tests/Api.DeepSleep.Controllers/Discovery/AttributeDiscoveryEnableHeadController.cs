namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;

    public class AttributeDiscoveryEnableHeadController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteEnableHead]
        public AttributeDiscoveryModel GetEnableHeadDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/true")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteEnableHead(enableHeadForGetRequests: true)]
        public AttributeDiscoveryModel GetEnableHeadTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/true/with/caching")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteEnableHead(enableHeadForGetRequests: true)]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        public AttributeDiscoveryModel GetEnableHeadTrueWithCaching()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/false")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteEnableHead(enableHeadForGetRequests: false)]
        public AttributeDiscoveryModel GetEnableHeadFalse()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/false/with/caching")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteEnableHead(enableHeadForGetRequests: false)]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        public AttributeDiscoveryModel GetEnableHeadFalseWithCaching()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
