namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryReadWriteController
    {
        [ApiRoute("GET", "discovery/attribute/readwrite/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteReadWriteConfiguration]
        public AttributeDiscoveryModel GetReadWriteDefault()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/readwrite/acceptheader/override")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteReadWriteConfiguration(acceptHeaderOverride: "application/xml")]
        public AttributeDiscoveryModel GetReadWriteAcceptHeaderOverride()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("GET", "discovery/attribute/readwrite/writeablemediatypes/override")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteReadWriteConfiguration(writeableMediaTypes: new[] { "application/xml" })]
        public AttributeDiscoveryModel GetReadWriteWriteableMediaTypesOverride()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute("POST", "discovery/attribute/readwrite/readablemediatypes/override")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [ApiRouteReadWriteConfiguration(readableMediaTypes: new[] { "application/xml" })]
        public AttributeDiscoveryModel GetReadWriteReadableMediaTypesOverride([BodyBound] AttributeDiscoveryModel request)
        {
            return request;
        }

        [ApiRoute("POST", "discovery/attribute/readwrite/readerresolver/override")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [CustomApiRouteReadWriteConfigurationWithReadResolver]
        public AttributeDiscoveryModel GetReadWriteReaderResolverOverride([BodyBound] string request)
        {
            return new AttributeDiscoveryModel
            {
                PostValue = request
            };
        }

        [ApiRoute("GET", "discovery/attribute/readwrite/writerresolver/override")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [CustomApiRouteReadWriteConfigurationWithWriteResolver]
        public string GetReadWriteWriterResolverOverride()
        {
            return "Posted";
        }
    }
}
