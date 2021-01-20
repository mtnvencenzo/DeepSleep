namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryIncludeRequestHeaderController
    {
        /// <summary>Gets the include request header default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/includeRequestHeader/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteIncludeRequestIdHeader]
        public AttributeDiscoveryModel GetIncludeRequestHeaderDefault()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the include request header true.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/includeRequestHeader/true")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteIncludeRequestIdHeader(includeRequestIdHeaderInResponse: true)]
        public AttributeDiscoveryModel GetIncludeRequestHeaderTrue()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the include request header false.</summary>
        /// <returns></returns>
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
