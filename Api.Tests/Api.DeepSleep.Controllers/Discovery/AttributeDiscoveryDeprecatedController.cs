namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryDeprecatedController
    {
        [ApiRoute(new[] { "GET" }, "discovery/attribute/deprecated")]
        public AttributeDiscoveryModel GetDeprecated()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/deprecated/true", true)]
        public AttributeDiscoveryModel GetDeprecatedTrue()
        {
            return new AttributeDiscoveryModel();
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/deprecated/false", false)]
        public AttributeDiscoveryModel GetDeprecatedFalse()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
