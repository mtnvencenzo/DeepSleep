namespace DeepSleep.OpenApi.NetCore.Controllers
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using DeepSleep.OpenApi.v3_0;

    /// <summary>
    /// 
    /// </summary>
    public class OpenApiController
    {
        private readonly IOpenApiGenerator generator;
        private readonly IApiRoutingTable routingTable;
        private readonly IApiRequestContextResolver requestContextResolver;

        /// <summary>Initializes a new instance of the <see cref="OpenApiController"/> class.</summary>
        /// <param name="generator">The generator.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="requestContextResolver">The request context resolver.</param>
        public OpenApiController(IOpenApiGenerator generator, IApiRoutingTable routingTable, IApiRequestContextResolver requestContextResolver)
        {
            this.generator = generator;
            this.routingTable = routingTable;
            this.requestContextResolver = requestContextResolver;
        }

        /// <summary>Documents this instance.</summary>
        /// <returns></returns>
        internal Task<OpenApiDocument3_0> Doc()
        {
            var document = this.generator.Generate(OpenApiVersion.V3, routingTable);

            var context = this.requestContextResolver.GetContext();

            // Force json serialization since the standard demands it
            context.RequestInfo.Accept = new MediaHeaderValueWithQualityString("application/json");

            return Task.FromResult(document);
        }
    }
}
