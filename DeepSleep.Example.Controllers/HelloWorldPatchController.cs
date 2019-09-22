using DeepSleep.Example.Controllers.Models;
using DeepSleep.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DeepSleep.Example.Controllers
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class HelloWorldPatchController
    {
        /// <summary>Gets the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public HelloWorldRs DoThePatch([BodyBound] List<JsonPatchOperation> body)
        {
            var document = new HelloWorldRs
            {
                Message = "Hello World"
            };


            body.ApplyTo(document, (err) =>
            {
            });


            return document;
        }
    }
}
