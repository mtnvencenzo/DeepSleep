namespace Api.DeepSleep.Controllers.Validation
{
    using global::DeepSleep;

    /// <summary>
    /// 
    /// </summary>
    public class ErrorResponseProviderController
    {
        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        [ApiRoute(new[] { "GET" }, "validationerrors/get")]
        [ApiRouteAllowAnonymous(allowAnonymous: true)]
        [CustomListErrorResponseProvider]
        public IApiResponse Get()
        {
            return ApiResponse.BadRequest(errors: new string[] { "Test Error" });
        }
    }
}
