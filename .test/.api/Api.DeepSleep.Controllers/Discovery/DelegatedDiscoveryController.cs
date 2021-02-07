namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Discovery;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Discovery.IDeepSleepRegistrationProvider" />
    public class DelegatedDiscoveryController : IDeepSleepRegistrationProvider
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="DelegatedDiscoveryController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <exception cref="ArgumentNullException">contextResolver</exception>
        public DelegatedDiscoveryController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
        }

        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        public DelegatedDiscoveryModel Get()
        {
            return new DelegatedDiscoveryModel();
        }

        Task<IList<DeepSleepRouteRegistration>> IDeepSleepRegistrationProvider.GetRoutes(IServiceProvider serviceProvider)
        {
            return Task.FromResult(new List<DeepSleepRouteRegistration>
            {
                new DeepSleepRouteRegistration(
                    template: "discovery/delegated",
                    httpMethods: new[] { "GET" },
                    controller: this.GetType(),
                    endpoint: nameof(Get))
            } as IList<DeepSleepRouteRegistration>);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DelegatedDiscoveryModel
    {
        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value => "Test";
    }
}
