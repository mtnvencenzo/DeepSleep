namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AttributeDiscoveryErrorResponseProviderController
    {
        private readonly IApiRequestContextResolver contextResolver;

        public AttributeDiscoveryErrorResponseProviderController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        [ApiRoute(new[] { "GET" }, "discovery/attribute/errorresponseprovider/default")]
        [ApiRouteAllowAnonymous(allowAnonymous: false)]
        [ApiAuthorization(authorizationProviderType: typeof(DefaultAuthorizationProvider))]
        [CustomApiRouteErrorResponseProvider]
        public IApiResponse GetRequestValidationtDefault()
        {
            var context = this.contextResolver.GetContext();

            context.AddValidationError("test-error-1");
            context.AddValidationError("test-error-2");

            return ApiResponse.BadRequest();
        }
    }
}
