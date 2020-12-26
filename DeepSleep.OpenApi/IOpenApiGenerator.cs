namespace DeepSleep.OpenApi
{
    using DeepSleep.Configuration;
    using DeepSleep.OpenApi.v3_0;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IOpenApiGenerator
    {
        /// <summary>Generates the specified version.</summary>
        /// <param name="version">The version.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        Task<OpenApiDocument3_0> Generate(OpenApiVersion version, IApiRoutingTable routingTable, IApiRequestConfiguration defaultRequestConfiguration);
    }
}
