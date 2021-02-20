namespace Samples.Simple.Api.Resources.HelloWorld
{
    using DeepSleep;
    using Samples.Simple.Api.Models;

    /// <summary>
    /// 
    /// </summary>
    public class HellowWorldResource
    {
        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        [ApiRoute(httpMethod: "GET", template: "/helloworld")]
        public HelloWorldRs Get()
        {
            return new HelloWorldRs();
        }
    }
}
