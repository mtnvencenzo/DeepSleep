namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Discovery;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DelegatedDiscoveryController : IRouteRegistrationProvider
    {
        public DelegatedDiscoveryModel Get()
        {
            return new DelegatedDiscoveryModel();
        }

        Task<IList<ApiRouteRegistration>> IRouteRegistrationProvider.GetRoutes()
        {
            return Task.FromResult(new List<ApiRouteRegistration>
            {
                new ApiRouteRegistration
                {
                    Template = "discovery/delegated",
                    HttpMethod = "GET",
                    Location = new ApiEndpointLocation
                    {
                        Controller = this.GetType(),
                        Endpoint = nameof(Get),
                        HttpMethod = "GET"
                    },
                    Configuration = null
                }
            } as IList<ApiRouteRegistration>);
        }
    }

    public class DelegatedDiscoveryModel
    {
        public string Value => "Test";
    }
}
