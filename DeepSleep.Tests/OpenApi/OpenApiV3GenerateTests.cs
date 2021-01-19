namespace DeepSleep.Tests.OpenApi
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.OpenApi;
    using DeepSleep.Tests.OpenApi.TestSetup;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Extensions;
    using Microsoft.OpenApi.Models;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiV3GenerateTests
    {
        [Fact]
        public async Task openapi___generates_v2_v3_for_json_and_yaml()
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

            var configuration = new OpenApiConfigurationProvider
            {
                Info = new OpenApiInfo
                {
                    Description = "Test",
                    Title = "Test"
                },
                IncludeHeadOperationsForGets = true,
                PrefixNamesWithNamespace = false
            };

            var document = await new OpenApiGenerator(configuration).Generate(table, null).ConfigureAwait(false);
            var resultsJsonV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);
            var resultsJsonV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
            var resultsYamlV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Yaml);
            var resultsYamlV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Yaml);
        }

        [Fact]
        public async Task openapi___generates_v2_v3_for_json_and_yaml_for_lists()
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

            var configuration = new OpenApiConfigurationProvider
            {
                Info = new OpenApiInfo
                {
                    Description = "Test",
                    Title = "Test"
                },
                IncludeHeadOperationsForGets = true,
                PrefixNamesWithNamespace = false
            };

            var document = await new OpenApiGenerator(configuration).Generate(table, null).ConfigureAwait(false);
            var resultsJsonV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);
            var resultsJsonV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
            var resultsYamlV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Yaml);
            var resultsYamlV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Yaml);
        }
    }
}
