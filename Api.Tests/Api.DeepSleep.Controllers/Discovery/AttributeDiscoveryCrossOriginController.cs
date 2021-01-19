namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryCrossOriginController
    {
        /// <summary>Gets the cross origin default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/crossorigin/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCrossOrigin()]
        public AttributeDiscoveryModel GetCrossOriginDefault()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the cross origin default with caching.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/crossorigin/default/with/caching")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        [ApiRouteCrossOrigin()]
        public AttributeDiscoveryModel GetCrossOriginDefaultWithCaching()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the cross origin specified empty.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/crossorigin/specified/empty")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCrossOrigin(allowCredentials: false, allowedHeaders: new[] { "" }, allowedOrigins: new[] { "" }, exposeHeaders: new[] { "" }, maxAgeSeconds: 100)]
        public AttributeDiscoveryModel GetCrossOriginSpecifiedEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the cross origin specified.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/crossorigin/specified")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCrossOrigin(allowCredentials: false, allowedHeaders: new[] { "Accept" }, allowedOrigins: new[] { "https://test.us" }, exposeHeaders: new[] { "X-RequestId" }, maxAgeSeconds: 100)]
        public AttributeDiscoveryModel GetCrossOriginSpecified()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the cross origin specified with cache control.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/crossorigin/specified/with/caching")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteCacheDirective(location: HttpCacheLocation.Public, cacheability: HttpCacheType.Cacheable, expirationSeconds: 120)]
        [ApiRouteCrossOrigin(allowCredentials: false, allowedHeaders: new[] { "Accept" }, allowedOrigins: new[] { "https://test.us" }, exposeHeaders: new[] { "X-RequestId" }, maxAgeSeconds: 100)]
        public AttributeDiscoveryModel GetCrossOriginSpecifiedWithCacheControl()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
