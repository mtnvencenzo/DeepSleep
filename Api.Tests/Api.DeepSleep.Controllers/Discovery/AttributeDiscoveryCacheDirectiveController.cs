namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;

    public class AttributeDiscoveryCacheDirectiveController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/cachedirective/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCacheDirective()]
        public AttributeDiscoveryModel GetCacheDirectiveDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/cachedirective/specified")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCacheDirective(
            location: HttpCacheLocation.Public, 
            cacheability: HttpCacheType.Cacheable, 
            expirationSeconds: 120,
            varyHeaderValue: "Test, Something")]
        public AttributeDiscoveryModel GetAuthorizationPolicyNotMatched()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
