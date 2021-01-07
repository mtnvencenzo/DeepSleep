namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryValidationErrorController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/default")]
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

        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/specified/empty")]
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

        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/specified/custom/no/replacements")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "uriBindingError-test",
            uriBindingValueError: "uriBindingValueError-test")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedCustomNoReplacements(int? queryValue, [UriBound] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/specified/custom/with/replacements")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "{paramName}",
            uriBindingValueError: "{paramName}-{paramValue}-{paramType}")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedCustomWithReplacements(int? queryValue, [UriBound] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/default/deserialization/error")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        public AttributeDiscoveryModel GetValidationErrorDefaultDeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/empty/deserialization/error")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            requestDeserializationError: "")]
        public AttributeDiscoveryModel GetValidationErrorEmptyDeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/custom/deserialization/error")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            requestDeserializationError: "Deserialization Failed")]
        public AttributeDiscoveryModel GetValidationErrorCustomDeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/450/deserialization/error")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            useCustomStatusForRequestDeserializationErrors: true)]
        public AttributeDiscoveryModel GetValidationErrorUse450DeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/400/deserialization/error")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteValidationErrorConfiguration(
            useCustomStatusForRequestDeserializationErrors: false)]
        public AttributeDiscoveryModel GetValidationErrorUse400DeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }
    }
}