namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryCacheDirectiveController
    {
        /// <summary>Gets the cache directive default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/cachedirective/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCacheDirective()]
        public AttributeDiscoveryModel GetCacheDirectiveDefault()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authorization policy not matched.</summary>
        /// <returns></returns>
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
