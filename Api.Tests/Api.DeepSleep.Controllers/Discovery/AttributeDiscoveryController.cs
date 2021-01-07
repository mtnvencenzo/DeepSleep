namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System;

    public class AttributeDiscoveryController
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public AttributeDiscoveryController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver ?? throw new ArgumentNullException(nameof(apiRequestContextResolver));
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute")]
        [ApiRouteEnableHead]
        public AttributeDiscoveryModel Get()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
