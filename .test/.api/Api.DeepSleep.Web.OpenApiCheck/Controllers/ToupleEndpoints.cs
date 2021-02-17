namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.OpenApi.Decorators;

    /// <summary>
    /// 
    /// </summary>
    public class ToupleEndpoints
    {
        /// <summary>Posts the touple simple.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(httpMethod: "POST", template: "/touple/simple")]
        [OasDescription("Post Touple Simple Custom Endpoint Description")]
        [OasResponse("201", typeof((string instring, bool inbool)))]
        [OasOperation(operationId: "PostToupleSimpleCustomOperation", tags: new[] { "Custom", "Touples" })]
        [return: OasDescription("The custom touple return value description")]
        public IApiResponse  PostToupleSimple([InBody] (string instring, bool inbool) request)
        {
            return ApiResponse.Created(value: request);
        }
    }
}
