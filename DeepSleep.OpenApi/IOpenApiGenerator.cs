namespace DeepSleep.OpenApi
{
    using DeepSleep.Configuration;
    using Microsoft.OpenApi.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IOpenApiGenerator
    {
        /// <summary>Generates the specified information.</summary>
        /// <param name="routingTable">The routing table.</param>
        /// <param name="defaultRequestConfiguration">The default request configuration.</param>
        /// <returns></returns>
        Task<OpenApiDocument> Generate(IApiRoutingTable routingTable, IApiRequestConfiguration defaultRequestConfiguration);
    }
}
