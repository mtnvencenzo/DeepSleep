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
                "POST_/test/basic/EndpointNoParams/{id}",
                "test/basic/EndpointNoParams/{id}",
                "POST",
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
                "GET_/test/list",
                "/test/list",
                "GET",
                typeof(ListController),
                nameof(ListController.List),
                new DefaultApiRequestConfiguration());

            table.AddRoute(
                "GET_/test/list1",
                "/test/list1",
                "GET",
                typeof(ListController),
                nameof(ListController.List1),
                new DefaultApiRequestConfiguration());

            table.AddRoute(
                "GET_/test/list2",
                "/test/list2",
                "GET",
                typeof(ListController),
                nameof(ListController.List2),
                new DefaultApiRequestConfiguration());

            table.AddRoute(
                "GET_/test/list/container",
                "/test/list/container",
                "GET",
                typeof(ListController),
                nameof(ListController.ListContainer),
                new DefaultApiRequestConfiguration());

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
