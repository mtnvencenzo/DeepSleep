namespace DeepSleep.OpenApi.Web.Controllers
{
    using DeepSleep.Configuration;
    using Microsoft.OpenApi.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiController
    {
        private readonly IOpenApiGenerator generator;
        private readonly IApiRoutingTable routingTable;
        private readonly IApiRequestConfiguration defaultRequestConfiguration;
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="OpenApiController"/> class.</summary>
        /// <param name="generator">The generator.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <param name="contextResolver">The context resolver.</param>
        public OpenApiController(
            IOpenApiGenerator generator,
            IApiRoutingTable routingTable,
            IApiRequestConfiguration defaultRequestConfiguration,
            IApiRequestContextResolver contextResolver)
        {
            this.generator = generator;
            this.routingTable = routingTable;
            this.defaultRequestConfiguration = defaultRequestConfiguration;
            this.contextResolver = contextResolver;
        }

        /// <summary>Documents the v2.</summary>
        /// <returns></returns>
        internal async Task<OpenApiDocument> DocV2()
        {
            var context = this.contextResolver.GetContext();

            context.TryAddItem("openapi_version", "2");

            var document = await this.generator.Generate(routingTable, defaultRequestConfiguration).ConfigureAwait(false);

            return document;
        }

        /// <summary>Documents the v3.</summary>
        /// <returns></returns>
        internal async Task<OpenApiDocument> DocV3()
        {
            var context = this.contextResolver.GetContext();

            context.TryAddItem("openapi_version", "3");

            var document = await this.generator.Generate(routingTable, defaultRequestConfiguration).ConfigureAwait(false);

            return document;
        }
    }
}
