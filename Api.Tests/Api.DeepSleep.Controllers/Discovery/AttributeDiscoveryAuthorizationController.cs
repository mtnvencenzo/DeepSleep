namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryAuthorizationController
    {
        /// <summary>Gets the authorization policy null.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/null")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthorizationPolicyNull()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authorization policy not matched.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/notmatched")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthorizationPolicyNotMatched()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authorization policy empty.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/empty")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        public AttributeDiscoveryModel GetAuthorizationPolicyEmpty()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the authorization policy default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/authorization/policy/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        public AttributeDiscoveryModel GetAuthorizationPolicyDefault()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
