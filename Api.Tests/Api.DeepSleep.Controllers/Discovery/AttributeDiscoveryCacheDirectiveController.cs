namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;

    public class AttributeDiscoveryCacheDirectiveController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/cachedirective/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCacheDirective()]
        public AttributeDiscoveryModel GetCacheDirectiveDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/cachedirective/specified")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
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
