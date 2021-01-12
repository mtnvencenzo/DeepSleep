namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryErrorResponseProviderController
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public AttributeDiscoveryErrorResponseProviderController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/errorresponseprovider/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
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
