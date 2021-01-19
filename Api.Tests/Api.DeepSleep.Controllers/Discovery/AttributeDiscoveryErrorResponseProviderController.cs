namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class AttributeDiscoveryErrorResponseProviderController
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="AttributeDiscoveryErrorResponseProviderController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public AttributeDiscoveryErrorResponseProviderController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets the request validationt default.</summary>
        /// <returns></returns>
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
