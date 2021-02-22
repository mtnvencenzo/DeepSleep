namespace Api.DeepSleep.Web.OpenApiCheck.Controllers
{
    using global::DeepSleep;

    public class ContextEndpoints
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="ContextEndpoints"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public ContextEndpoints(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets the request.</summary>
        /// <returns></returns>
        [ApiRoute(httpMethod: "GET", template: "/request")]
        public RequestModel GetRequest()
        {
            var context = contextResolver.GetContext();

            return new RequestModel
            {
                Path = context.Request.Path,
                RequestUri = context.Request.RequestUri
            };
        }
    }

    public class RequestModel
    {
        public string Path { get; set; }

        public string RequestUri { get; set; }
    }
}
