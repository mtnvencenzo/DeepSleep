namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Configuration;

    public class AttributeDiscoveryCrossOriginController
    {
        [ApiRoute("GET", "discovery/attribute/crossorigin/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCrossOrigin()]
        public AttributeDiscoveryModel GetCrossOriginDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/crossorigin/default/with/caching")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        [ApiRouteCrossOrigin()]
        public AttributeDiscoveryModel GetCrossOriginDefaultWithCaching()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/crossorigin/specified/empty")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCrossOrigin(allowCredentials: false, allowedHeaders: new[] { "" }, allowedOrigins: new[] { "" }, exposeHeaders: new[] { "" }, maxAgeSeconds: 100)]
        public AttributeDiscoveryModel GetCrossOriginSpecifiedEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/crossorigin/specified")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCrossOrigin(allowCredentials: false, allowedHeaders: new[] { "Accept" }, allowedOrigins: new[] { "https://test.us" }, exposeHeaders: new[] { "X-RequestId" }, maxAgeSeconds: 100)]
        public AttributeDiscoveryModel GetCrossOriginSpecified()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/crossorigin/specified/with/caching")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        [ApiRouteCrossOrigin(allowCredentials: false, allowedHeaders: new[] { "Accept" }, allowedOrigins: new[] { "https://test.us" }, exposeHeaders: new[] { "X-RequestId" }, maxAgeSeconds: 100)]
        public AttributeDiscoveryModel GetCrossOriginSpecifiedWithCacheControl()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
