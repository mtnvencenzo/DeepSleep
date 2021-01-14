namespace DeepSleep.OpenApi.Web.Controllers
{
    using DeepSleep.Configuration;
    using DeepSleep.OpenApi.v3_0;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiController
    {
        private readonly IOpenApiGenerator generator;
        private readonly IApiRoutingTable routingTable;
        private readonly IApiRequestContextResolver requestContextResolver;
        private readonly IApiRequestConfiguration defaultRequestConfiguration;

        /// <summary>Initializes a new instance of the <see cref="OpenApiController"/> class.</summary>
        /// <param name="generator">The generator.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="requestContextResolver">The request context resolver.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        public OpenApiController(
            IOpenApiGenerator generator,
            IApiRoutingTable routingTable,
            IApiRequestContextResolver requestContextResolver,
            IApiRequestConfiguration defaultRequestConfiguration)
        {
            this.generator = generator;
            this.routingTable = routingTable;
            this.requestContextResolver = requestContextResolver;
            this.defaultRequestConfiguration = defaultRequestConfiguration;
        }

        /// <summary>Documents this instance.</summary>
        /// <returns></returns>
        internal async Task<OpenApiDocument3_0> Doc()
        {
            var document = await this.generator.Generate(OpenApiVersion.V3, routingTable, defaultRequestConfiguration).ConfigureAwait(false);

            return document;
        }
    }
}
