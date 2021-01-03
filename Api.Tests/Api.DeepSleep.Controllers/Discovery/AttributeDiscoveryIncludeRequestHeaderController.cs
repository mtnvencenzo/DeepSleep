namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryIncludeRequestHeaderController
    {
        [ApiRoute("GET", "discovery/attribute/includeRequestHeader/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteIncludeRequestIdHeader]
        public AttributeDiscoveryModel GetIncludeRequestHeaderDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/includeRequestHeader/true")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteIncludeRequestIdHeader(includeRequestIdHeaderInResponse: true)]
        public AttributeDiscoveryModel GetIncludeRequestHeaderTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/includeRequestHeader/false")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteIncludeRequestIdHeader(includeRequestIdHeaderInResponse: false)]
        public AttributeDiscoveryModel GetIncludeRequestHeaderFalse()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
