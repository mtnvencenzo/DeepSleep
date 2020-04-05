namespace DeepSleep.OpenApi.Tests
{
    using DeepSleep.Configuration;
    using DeepSleep.OpenApi.Tests.TestSetup;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiV3GenerateTests
    {
        [Fact]
        public void Test1()
        {
            var table = new DefaultApiRoutingTable();
            table.AddRoute(
                "GET_/test/basic/EndpointNoParams/{id}",
                "/test/basic/EndpointNoParams/{id}",
                "GET",
                typeof(BasicController),
                nameof(BasicController.EndpointNoParams),
                new DefaultApiRequestConfiguration());

            table.AddRoute(
                "PATCH_/test/basic/EndpointNoParams/{id}",
                "/test/basic/EndpointNoParams/{id}",
                "PATCH",
                typeof(BasicController),
                nameof(BasicController.EndpointNoParamsPatch),
                new DefaultApiRequestConfiguration());

            table.AddRoute(
                "/test/basic/EndpointWithRouteParam/{name}",
                "/test/basic/EndpointWithRouteParam/{name}",
                "GET",
                typeof(BasicController),
                nameof(BasicController.EndpointWithRouteParam),
                new DefaultApiRequestConfiguration());

            table.AddRoute(
                "/test/basic/EndpointWithBodyParam",
                "/test/basic/EndpointWithBodyParam",
                "POST",
                typeof(BasicController),
                nameof(BasicController.EndpointWithBodyParam),
                new DefaultApiRequestConfiguration());

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
