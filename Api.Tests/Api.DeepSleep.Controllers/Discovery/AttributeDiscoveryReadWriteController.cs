namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryReadWriteController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteReadWriteConfiguration]
        public AttributeDiscoveryModel GetReadWriteDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/acceptheader/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteReadWriteConfiguration(acceptHeaderOverride: "application/xml")]
        public AttributeDiscoveryModel GetReadWriteAcceptHeaderOverride()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/writeablemediatypes/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteReadWriteConfiguration(writeableMediaTypes: new[] { "application/xml" })]
        public AttributeDiscoveryModel GetReadWriteWriteableMediaTypesOverride()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/readwrite/readablemediatypes/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteReadWriteConfiguration(readableMediaTypes: new[] { "application/xml" })]
        public AttributeDiscoveryModel GetReadWriteReadableMediaTypesOverride([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute(new[] { "POST" }, "discovery/attribute/readwrite/readerresolver/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [CustomApiRouteReadWriteConfigurationWithReadResolver]
        public AttributeDiscoveryModel GetReadWriteReaderResolverOverride([BodyBound] string request)
        {
            return new AttributeDiscoveryModel
            {
                PostValue = request
            };
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/writerresolver/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [CustomApiRouteReadWriteConfigurationWithWriteResolver]
        public string GetReadWriteWriterResolverOverride()
        {
            return "Posted";
        }
    }
}
