namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryRequestValidationController
    {
        [ApiRoute("POST", "discovery/attribute/requestvalidation/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteRequestValidation]
        public AttributeDiscoveryModel GetRequestValidationtDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("POST", "discovery/attribute/requestvalidation/specified")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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

        [ApiRoute("POST", "discovery/attribute/requestvalidation/specified/require/contentlength")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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

        [ApiRoute("POST", "discovery/attribute/requestvalidation/specified/requestbody/not/allowed")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
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
