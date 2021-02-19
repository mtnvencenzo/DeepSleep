namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.OpenApi.Decorators;
    using System;
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
        public void PostBasicObjectModelReturnVoid([InBody] BasicObject request)
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
        public Task PostBasicObjectModelReturnTask([InBody] BasicObject request)
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
        public Task<IApiResponse> PostBasicObjectModelReturnTaskWith202Attribute([InBody] BasicObject request)
        {
            return Task.FromResult(ApiResponse.Accepted());
        }

        /// <summary>Posts the basic object model return task with200 default attribute.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiRoute(httpMethod: "POST", template: "/basic/object/model/return/task/with/200/default/attribute")]
        [OasResponse(responseType: typeof(void))]
        public Task<IApiResponse> PostBasicObjectModelReturnTaskWith200DefaultAttribute([InBody] BasicObject request)
        {
            return Task.FromResult(ApiResponse.Ok());
        }

        /// <summary>Obsoletes this instance.</summary>
        /// <returns></returns>
        [Obsolete]
        [ApiRoute(httpMethod: "GET", template: "/obsolete")]
        public Task Obsolete()
        {
            return Task.FromResult(ApiResponse.Ok());
        }
    }
}
