namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Discovery;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DelegatedDiscoveryController : IRouteRegistrationProvider
    {
        private readonly IApiRequestContextResolver contextResolver;

        public DelegatedDiscoveryController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver ?? throw new ArgumentNullException(nameof(contextResolver));
        }

        public DelegatedDiscoveryModel Get()
        {
            return new DelegatedDiscoveryModel();
        }

        Task<IList<ApiRouteRegistration>> IRouteRegistrationProvider.GetRoutes(IServiceProvider serviceProvider)
        {
            return Task.FromResult(new List<ApiRouteRegistration>
            {
                new ApiRouteRegistration(
                    template: "discovery/delegated",
                    httpMethods: new[] { "GET" },
                    controller: this.GetType(),
                    endpoint: nameof(Get))
            } as IList<ApiRouteRegistration>);
        }
    }

    public class DelegatedDiscoveryModel
    {
        public string Value => "Test";
    }
}
