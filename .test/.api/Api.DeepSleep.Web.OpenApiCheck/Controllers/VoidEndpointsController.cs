namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.OpenApi.Decorators;
    using System.Threading.Tasks;

    /// <summary>
    /// This is the collection of enpoints that represent void endpoints.
    /// </summary>
    /// <remarks>
    /// This is the description for the void endpoints collection
    /// </remarks>
    public class VoidEndpointsController
    {
        /// <summary>Posts the basic object model with void return.</summary>
        /// <param name="request">The request.</param>
        /// <remarks>
        /// Offically posts the basic object model with void return.
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [ApiRoute(httpMethod: "POST", template: "/basic/object/model/return/void")]
        public void PostBasicObjectModelReturnVoid([BodyBound] BasicObject request)
        {
        }

        /// <summary>Posts the basic object model with task return.</summary>
        /// <param name="request">The request.</param>
        /// <remarks>
        /// Offically posts the basic object model with task return.
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [ApiRoute(httpMethod: "POST", template: "/basic/object/model/return/task")]
        public Task PostBasicObjectModelReturnTask([BodyBound] BasicObject request)
        {
            return Task.CompletedTask;
        }

        /// <summary>Posts the basic object model with task 202 return.</summary>
        /// <param name="request">The request.</param>
        /// <remarks>
        /// Offically posts the basic object model with task 202 return.
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [ApiRoute(httpMethod: "POST", template: "/basic/object/model/return/task/with/202/attribute")]
        [OasResponse(statusCode: "202", responseType: typeof(void))] 
        public Task<IApiResponse> PostBasicObjectModelReturnTaskWith202Attribute([BodyBound] BasicObject request)
        {
            return Task.FromResult(ApiResponse.Accepted());
        }
    }
}
