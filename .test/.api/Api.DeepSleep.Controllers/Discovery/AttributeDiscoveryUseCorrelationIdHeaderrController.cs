namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryUseCorrelationIdHeaderrController
    {
        /// <summary>Gets the use correlation identifier header default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/usecorrelationidheader/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteUseCorrelationIdHeader]
        public AttributeDiscoveryModel GetUseCorrelationIdHeaderDefault()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the use correlation identifier header true.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/usecorrelationidheader/true")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteUseCorrelationIdHeader(useCorrelationIdHeader: true)]
        public AttributeDiscoveryModel GetUseCorrelationIdHeaderTrue()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the use correlation identifier header false.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/usecorrelationidheader/false")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteUseCorrelationIdHeader(useCorrelationIdHeader: false)]
        public AttributeDiscoveryModel GetUseCorrelationIdHeaderFalse()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
