namespace DeepSleep.OpenApi.Tests
{
    using DeepSleep.OpenApi.Tests.TestSetup;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiV3GenerateTests
    {
        [Fact]
        public async Task Test1()
        {
            var table = new DefaultApiRoutingTable();
            await table.AddRoute(
                "/test/basic/EndpointNoParams",
                "/test/basic/EndpointNoParams",
                "GET",
                typeof(BasicController),
                nameof(BasicController.EndpointNoParams),
                new ApiResourceConfig());

            var generator = new DefaultOpenApiGenerator();

            var document = generator.Generate(OpenApiVersion.V3, table);

            var results = JsonConvert.SerializeObject(
                document,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });

            System.Diagnostics.Debug.Write(results);
        }
    }
}
