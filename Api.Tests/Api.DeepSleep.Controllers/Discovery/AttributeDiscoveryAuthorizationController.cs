namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryAuthorizationController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/null")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthorizationPolicyNull()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/notmatched")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        //[ApiRouteAuthorization(policy: "notmatched")]
        public AttributeDiscoveryModel GetAuthorizationPolicyNotMatched()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/empty")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        //[ApiRouteAuthorization(policy: "")]
        public AttributeDiscoveryModel GetAuthorizationPolicyEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        public AttributeDiscoveryModel GetAuthorizationPolicyDefault()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
