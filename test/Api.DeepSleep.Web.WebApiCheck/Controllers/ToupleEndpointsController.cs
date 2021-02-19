namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class ToupleEndpointsController
    {
        /// <summary>Posts the touple simple.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(template: "/touple/simple", Name = "PostToupleSimple")]
        public (string instring, bool inbool) PostToupleSimple([FromBody] (string instring, bool inbool) request)
        {
            return request;
        }
    }
}
