namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;

    public class AttributeDiscoveryEnableHeadController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteEnableHead]
        public AttributeDiscoveryModel GetEnableHeadDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/true")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteEnableHead(enableHeadForGetRequests: true)]
        public AttributeDiscoveryModel GetEnableHeadTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/true/with/caching")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteEnableHead(enableHeadForGetRequests: true)]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        public AttributeDiscoveryModel GetEnableHeadTrueWithCaching()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/false")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteEnableHead(enableHeadForGetRequests: false)]
        public AttributeDiscoveryModel GetEnableHeadFalse()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/enablehead/false/with/caching")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteEnableHead(enableHeadForGetRequests: false)]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        public AttributeDiscoveryModel GetEnableHeadFalseWithCaching()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
