namespace Api.DeepSleep.Web.WebApiCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    /// <summary>
    /// This is the collection of enpoints that represent void endpoints.
    /// </summary>
    /// <remarks>
    /// This is the description for the void endpoints collection
    /// </remarks>
    [ApiController]
    [Route("[controller]")]
    public class VoidEndpointsController : ControllerBase
    {
        /// <summary>Posts the basic object model with void return.</summary>
        /// <param name="request">The request.</param>
        /// <remarks>
        /// Offically posts the basic object model with void return.
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [HttpPost]
        [Route(template: "/basic/object/model/return/void", Name = "PostBasicObjectModelReturnVoid")]
        public void PostBasicObjectModelReturnVoid([FromBody] BasicObject request)
        {
        }

        /// <summary>Posts the basic object model with task return.</summary>
        /// <param name="request">The request.</param>
        /// <remarks>
        /// Offically posts the basic object model with task return.
        /// Here's some documentation:
        /// <a href="http://www.google.com" />.
        /// </remarks>
        [HttpPost]
        [Route(template: "/basic/object/model/return/task", Name = "PostBasicObjectModelReturnTask")]
        public Task PostBasicObjectModelReturnTask([FromBody] BasicObject request)
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
        [HttpPost]
        [Route(template: "/basic/object/model/return/task/with/202/attribute", Name = "PostBasicObjectModelReturnTaskWith202Attribute")]
        [ProducesResponseType(statusCode: 202, type: typeof(void))]
        public Task<IActionResult> PostBasicObjectModelReturnTaskWith202Attribute([FromBody] BasicObject request)
        {
            return Task.FromResult(Accepted() as IActionResult);
        }
    }
}
