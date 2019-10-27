namespace DeepSleep.OpenApi
{
    using DeepSleep.OpenApi.v3_0;

    /// <summary>
    /// 
    /// </summary>
    public interface IOpenApiGenerator
    {
        /// <summary>
        /// Generates the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="routingTable">The routing table.</param>
        /// <returns></returns>
        OpenApiDocument3_0 Generate(OpenApiVersion version, IApiRoutingTable routingTable);
    }
}
