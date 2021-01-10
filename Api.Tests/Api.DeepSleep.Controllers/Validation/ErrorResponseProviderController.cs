namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;

    public class ErrorResponseProviderController
    {
        [ApiRoute(new[] { "GET" }, "validationerrors/get")]
        [ApiRouteAuthentication(allowAnonymous: true)]
        [CustomListErrorResponseProvider]
        public IApiResponse Get()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }
    }
}
