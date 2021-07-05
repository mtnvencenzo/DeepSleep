namespace Samples.Simple.Api.Resources.Cors
{
    using DeepSleep;
    using Samples.Simple.Api.Models;

    public class CorsResource
    {
        [ApiRoute(httpMethod: "GET", template: "/cors/simple")]
        [ApiRouteAllowAnonymous]
        [ApiRouteCrossOrigin(
            allowCredentials: true,
            allowedHeaders: new[] { "Content-Type", "Authorization" },
            allowedOrigins: new[] { "https://localhost:5001" },
            maxAgeSeconds: 100,
            exposeHeaders: new[] { "X-Request-Id" })]
        public HelloWorldRs CorsSimpleGet()
        {
            return new HelloWorldRs();
        }

        [ApiRoute(httpMethods: new[] { "POST", "PUT" }, template: "/cors/requiring/preflight")]
        [ApiRouteAllowAnonymous]
        [ApiRouteCrossOrigin(
            allowCredentials: true,
            allowedHeaders: new[] { "Content-Type", "Authorization" },
            allowedOrigins: new[] { "https://localhost:5001" },
            maxAgeSeconds: 100,
            exposeHeaders: new[] { "X-Request-Id" })]
        public HelloWorldRs CorsRequiringPreflight()
        {
            return new HelloWorldRs();
        }
    }
}
