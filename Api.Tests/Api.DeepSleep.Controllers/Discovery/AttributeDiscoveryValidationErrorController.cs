namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;

    public class AttributeDiscoveryValidationErrorController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
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
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
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
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
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
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
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
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecificationWithCustomDeserializationStatus)]
        public AttributeDiscoveryModel GetValidationErrorDefaultDeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/empty/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            requestDeserializationError: "",
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecificationWithCustomDeserializationStatus)]
        public AttributeDiscoveryModel GetValidationErrorEmptyDeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/custom/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            requestDeserializationError: "Deserialization Failed",
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecificationWithCustomDeserializationStatus)]
        public AttributeDiscoveryModel GetValidationErrorCustomDeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/450/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecificationWithCustomDeserializationStatus)]
        public AttributeDiscoveryModel GetValidationErrorUse450DeserializationError([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/400/deserialization/error/for/strict")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.StrictHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorUse400DeserializationErrorForStrict([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/400/deserialization/error/for/common")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.StrictHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorUse400DeserializationErrorForCommon([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }
    }
}