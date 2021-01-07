namespace DeepSleep.OpenApi.Tests
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.OpenApi.Tests.TestSetup;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiV3GenerateTests
    {
        [Fact]
        public async Task Test1()
        {
            var table = new DefaultApiRoutingTable();
            table.AddRoute(new ApiRouteRegistration(
                template: "/test/basic/EndpointNoParams/{id}",
                httpMethods: new[] { "GET" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParams),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "/test/basic/EndpointNoParams/{id}",
                httpMethods: new[] { "PATCH" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParamsPatch),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "test/basic/EndpointNoParams/{id}",
                httpMethods: new[] { "POST" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParamsPatch),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "/test/basic/EndpointWithRouteParam/{name}",
                httpMethods: new[] { "GET" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointWithRouteParam),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "/test/basic/EndpointWithBodyParam",
                httpMethods: new[] { "POST" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointWithBodyParam),
                config: new DefaultApiRequestConfiguration()));

            var generator = new DefaultOpenApiGenerator();

            var document = await generator.Generate(OpenApiVersion.V3, table, null).ConfigureAwait(false);

            var results = JsonSerializer.Serialize(document, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            System.Diagnostics.Debug.Write(results);
        }

        [Fact]
        public async Task TestList()
        {
            var table = new DefaultApiRoutingTable();
            table.AddRoute(new ApiRouteRegistration(
                template: "/test/list",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.List),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "/test/list1",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.List1),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "/test/list2",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.List2),
                config: new DefaultApiRequestConfiguration()));

            table.AddRoute(new ApiRouteRegistration(
                template: "/test/list/container",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.ListContainer),
                config: new DefaultApiRequestConfiguration()));

            var generator = new DefaultOpenApiGenerator();

            DefaultOpenApiGenerator.PrefixNamesWithNamespace = false;

            var document = await generator.Generate(OpenApiVersion.V3, table, null).ConfigureAwait(false);

            var results = JsonSerializer.Serialize(document, new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            System.Diagnostics.Debug.Write(results);
        }
    }
}
