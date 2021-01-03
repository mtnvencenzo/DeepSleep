namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryAuthorizationController
    {
        [ApiRoute("GET", "discovery/attribute/authorization/policy/null")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: null)]
        public AttributeDiscoveryModel GetAuthorizationPolicyNull()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/authorization/policy/notmatched")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "notmatched")]
        public AttributeDiscoveryModel GetAuthorizationPolicyNotMatched()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/authorization/policy/empty")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "")]
        public AttributeDiscoveryModel GetAuthorizationPolicyEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/authorization/policy/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        public AttributeDiscoveryModel GetAuthorizationPolicyDefault()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
