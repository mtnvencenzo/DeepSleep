namespace Samples.Simple.Api.Resources.HelloWorld
{
    using DeepSleep;
    using DeepSleep.Auth;
    using Samples.Simple.Api.Models;

    public class HellowWorldAuthenticatedResource
    {
        [ApiRoute(httpMethod: "GET", template: "/helloworld/authenticate")]
        [ApiAuthentication(typeof(StaticTokenAuthenticationProvider))]
        public HelloWorldRs HelloWorldAuthenticated()
        {
            return new HelloWorldRs();
        }
    }
}
