namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryController
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="AttributeDiscoveryController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <exception cref="ArgumentNullException">contextResolver</exception>
        public AttributeDiscoveryController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
        }

        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "discovery/attribute")]
        [ApiRouteEnableHead]
        public AttributeDiscoveryModel Get()
        {
            return new AttributeDiscoveryModel();
        }
    }
}
