namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryRequestValidationController
    {
        [ApiRoute(new[] { "POST" }, "discovery/attribute/requestvalidation/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteRequestValidation]
        public AttributeDiscoveryModel GetRequestValidationtDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/requestvalidation/specified")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteRequestValidation(
            maxHeaderLength: 100,
            maxRequestLength: 100,
            maxRequestUriLength: 150,
            requireContentLengthOnRequestBodyRequests: false,
            allowRequestBodyWhenNoModelDefined: true)]
        public AttributeDiscoveryModel GetRequestValidationtSpecified()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/requestvalidation/specified/require/contentlength")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteRequestValidation(
            maxHeaderLength: 100,
            maxRequestLength: 100,
            maxRequestUriLength: 180,
            requireContentLengthOnRequestBodyRequests: true,
            allowRequestBodyWhenNoModelDefined: true)]
        public AttributeDiscoveryModel GetRequestValidationRequiresContentLength()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/requestvalidation/specified/requestbody/not/allowed")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteRequestValidation(
            maxHeaderLength: 100,
            maxRequestUriLength: 180,
            requireContentLengthOnRequestBodyRequests: true,
            allowRequestBodyWhenNoModelDefined: false)]
        public AttributeDiscoveryModel GetRequestValidationRequestBodyNotAllowed()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
