namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;

    public class AttributeDiscoveryErrorResponseProviderController
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public AttributeDiscoveryErrorResponseProviderController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        [ApiRoute("GET", "discovery/attribute/errorresponseprovider/default")]
        [ApiRouteAuthentication(allowAnonymous: false)]
        [ApiRouteAuthorization(policy: "Default")]
        [CustomApiRouteErrorResponseProvider]
        public IApiResponse GetRequestValidationtDefault()
        {
            var context = this.apiRequestContextResolver.GetContext();

            context.AddValidationError("test-error-1");
            context.AddValidationError("test-error-2");

            return ApiResponse.BadRequest();
        }
    }
}
