namespace Api.DeepSleep.Controllers.ValidationErrors
{
    using global::DeepSleep;

    public class ValidationErrorsController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        public ValidationErrorsController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        [ApiRoute(new[] { "GET" }, "validationerrors/get")]
        [ApiRouteAuthentication(allowAnonymous: true)]
        [CustomListErrorResponseProvider]
        public IApiResponse Get()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }
    }
}
