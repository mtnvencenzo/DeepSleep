namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryValidationErrorController
    {
        [ApiRoute("GET", "discovery/attribute/validationerror/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration]
        public AttributeDiscoveryModel GetValidationErrorDefault(int? queryValue, [UriBound] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        [ApiRoute("GET", "discovery/attribute/validationerror/specified/empty")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "", 
            uriBindingValueError: "")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedEmpty(int? queryValue, [UriBound] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        [ApiRoute("GET", "discovery/attribute/validationerror/specified/custom/no/replacements")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "1|uriBindingError-test",
            uriBindingValueError: "2|uriBindingValueError-test")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedCustomNoReplacements(int? queryValue, [UriBound] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        [ApiRoute("GET", "discovery/attribute/validationerror/specified/custom/with/replacements")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "1|{paramName}",
            uriBindingValueError: "2|{paramName}-{paramValue}-{paramType}")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedCustomWithReplacements(int? queryValue, [UriBound] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }
    }
}