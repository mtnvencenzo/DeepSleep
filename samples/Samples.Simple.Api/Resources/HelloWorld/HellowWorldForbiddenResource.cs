namespace Samples.Simple.Api.Resources.HelloWorld
{
    using DeepSleep;
    using DeepSleep.Auth;
    using Samples.Simple.Api.Models;

    public class HellowWorldForbiddenResource
    {
        [ApiRoute(httpMethod: "GET", template: "/helloworld/authorize")]
        [ApiAuthentication(typeof(StaticTokenAuthenticationProvider))]
        [ApiAuthorization(typeof(AdminRoleAuthorizationProvider))]
        public HelloWorldRs HelloWorldForbidden()
        {
            return new HelloWorldRs();
        }
    }
}
