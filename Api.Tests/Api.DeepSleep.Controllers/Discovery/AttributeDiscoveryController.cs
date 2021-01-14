namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System;

    public class AttributeDiscoveryController
    {
        private readonly IApiRequestContextResolver contextResolver;

        public AttributeDiscoveryController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute")]
        [ApiRouteEnableHead]
        public AttributeDiscoveryModel Get()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
