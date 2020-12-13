namespace DeepSleep.OpenApi.Tests
{
    using DeepSleep.Configuration;
    using DeepSleep.OpenApi.Tests.TestSetup;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Xunit;

    public class OpenApiV3GenerateTests
    {
        [Fact]
        public void Test1()
        {
            var table = new DefaultApiRoutingTable();
            table.AddRoute(
                template: "/test/basic/EndpointNoParams/{id}",
                httpMethod: "GET",
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParams),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "/test/basic/EndpointNoParams/{id}",
                httpMethod: "PATCH",
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParamsPatch),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "test/basic/EndpointNoParams/{id}",
                httpMethod: "POST",
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParamsPatch),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "/test/basic/EndpointWithRouteParam/{name}",
                httpMethod: "GET",
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointWithRouteParam),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "/test/basic/EndpointWithBodyParam",
                httpMethod: "POST",
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointWithBodyParam),
                config: new DefaultApiRequestConfiguration());

            var generator = new DefaultOpenApiGenerator();

            var document = generator.Generate(OpenApiVersion.V3, table);

            var results = JsonSerializer.Serialize(document, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            System.Diagnostics.Debug.Write(results);
        }

        [Fact]
        public void TestList()
        {
            var table = new DefaultApiRoutingTable();
            table.AddRoute(
                template: "/test/list",
                httpMethod: "GET",
                controller: typeof(ListController),
                endpoint: nameof(ListController.List),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "/test/list1",
                httpMethod: "GET",
                controller: typeof(ListController),
                endpoint: nameof(ListController.List1),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "/test/list2",
                httpMethod: "GET",
                controller: typeof(ListController),
                endpoint: nameof(ListController.List2),
                config: new DefaultApiRequestConfiguration());

            table.AddRoute(
                template: "/test/list/container",
                httpMethod: "GET",
                controller: typeof(ListController),
                endpoint: nameof(ListController.ListContainer),
                config: new DefaultApiRequestConfiguration());

            var generator = new DefaultOpenApiGenerator();

            DefaultOpenApiGenerator.PrefixNamesWithNamespace = false;
            var document = generator.Generate(OpenApiVersion.V3, table);

            var results = JsonSerializer.Serialize(document, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            System.Diagnostics.Debug.Write(results);
        }
    }
}
