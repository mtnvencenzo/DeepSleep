namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryDeprecatedController
    {
        /// <summary>Gets the deprecated.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/deprecated")]
        public AttributeDiscoveryModel GetDeprecated()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the deprecated true.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/deprecated/true", true)]
        public AttributeDiscoveryModel GetDeprecatedTrue()
        {
            return new AttributeDiscoveryModel();
        }

        /// <summary>Gets the deprecated false.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute/deprecated/false", false)]
        public AttributeDiscoveryModel GetDeprecatedFalse()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
