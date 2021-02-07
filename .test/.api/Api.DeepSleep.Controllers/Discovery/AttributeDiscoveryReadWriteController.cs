namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryReadWriteController
    {
        /// <summary>Gets the read write default.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteMediaSerializerConfiguration]
        public AttributeDiscoveryModel GetReadWriteDefault()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the read write accept header override.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/acceptheader/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteMediaSerializerConfiguration(acceptHeaderOverride: "application/xml")]
        public AttributeDiscoveryModel GetReadWriteAcceptHeaderOverride()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the read write writeable media types override.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/readwrite/writeablemediatypes/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteMediaSerializerConfiguration(writeableMediaTypes: new[] { "application/xml" })]
        public AttributeDiscoveryModel GetReadWriteWriteableMediaTypesOverride()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the read write readable media types override.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(new[] { "POST" }, "discovery/attribute/readwrite/readablemediatypes/override")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [ApiRouteMediaSerializerConfiguration(readableMediaTypes: new[] { "application/xml" })]
        public AttributeDiscoveryModel GetReadWriteReadableMediaTypesOverride([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        /// <summary>Gets the read write reader resolver override.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
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

        /// <summary>Gets the read write writer resolver override.</summary>
        /// <returns></returns>
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
