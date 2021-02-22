namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using global::DeepSleep.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryValidationErrorController
    {
        /// <summary>Gets the validation error default.</summary>
        /// <param name="queryValue">The query value.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration]
        public AttributeDiscoveryModel GetValidationErrorDefault(int? queryValue, [InUri] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        /// <summary>Gets the validation error specified empty.</summary>
        /// <param name="queryValue">The query value.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/specified/empty")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "",
            uriBindingValueError: "")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedEmpty(int? queryValue, [InUri] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        /// <summary>Gets the validation error specified custom no replacements.</summary>
        /// <param name="queryValue">The query value.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/specified/custom/no/replacements")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "uriBindingError-test",
            uriBindingValueError: "uriBindingValueError-test")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedCustomNoReplacements(int? queryValue, [InUri] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        /// <summary>Gets the validation error specified custom with replacements.</summary>
        /// <param name="queryValue">The query value.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/validationerror/specified/custom/with/replacements")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            uriBindingError: "{paramName}",
            uriBindingValueError: "{paramName}-{paramValue}-{paramType}")]
        public AttributeDiscoveryModel GetValidationErrorSpecifiedCustomWithReplacements(int? queryValue, [InUri] AttributeDiscoveryModel request)
        {
            return new AttributeDiscoveryModel
            {
                CustomInt = queryValue ?? request?.CustomInt ?? 0,
                PostValue = request?.PostValue ?? string.Empty
            };
        }

        /// <summary>Gets the validation error default deserialization error.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/default/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorDefaultDeserializationError([InBody] AttributeDiscoveryModel request)
        {
            return request;
        }

        /// <summary>Gets the validation error empty deserialization error.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/empty/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            requestDeserializationError: "",
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorEmptyDeserializationError([InBody] AttributeDiscoveryModel request)
        {
            return request;
        }

        /// <summary>Gets the validation error custom deserialization error.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/custom/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            requestDeserializationError: "Deserialization Failed",
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorCustomDeserializationError([InBody] AttributeDiscoveryModel request)
        {
            return request;
        }

        /// <summary>Gets the validation error use450 deserialization error.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/450/deserialization/error")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.CommonHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorUse450DeserializationError([InBody] AttributeDiscoveryModel request)
        {
            return request;
        }

        /// <summary>Gets the validation error use400 deserialization error for strict.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/400/deserialization/error/for/strict")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.StrictHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorUse400DeserializationErrorForStrict([InBody] AttributeDiscoveryModel request)
        {
            return request;
        }

        /// <summary>Gets the validation error use400 deserialization error for common.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/validationerror/400/deserialization/error/for/common")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteValidationErrorConfiguration(
            httpStatusMode: ValidationHttpStatusMode.StrictHttpSpecification)]
        public AttributeDiscoveryModel GetValidationErrorUse400DeserializationErrorForCommon([InBody] AttributeDiscoveryModel request)
        {
            return request;
        }
    }
}