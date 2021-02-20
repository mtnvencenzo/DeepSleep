namespace DeepSleep.OpenApi.Tests
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.OpenApi;
    using DeepSleep.OpenApi.Tests.TestSetup;
    using Microsoft.OpenApi;
    using Microsoft.OpenApi.Extensions;
    using Microsoft.OpenApi.Models;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiV3GenerateTests
    {
        [Fact]
        public async Task openapi___generates_v2_v3_for_json_and_yaml()
        {
            var table = new ApiRoutingTable();
            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/basic/EndpointNoParams/{id}",
                httpMethods: new[] { "GET" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParams),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/basic/EndpointNoParams/{id}",
                httpMethods: new[] { "PATCH" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParamsPatch),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "test/basic/EndpointNoParams/{id}",
                httpMethods: new[] { "POST" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointNoParamsPatch),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/basic/EndpointWithRouteParam/{name}",
                httpMethods: new[] { "GET" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointWithRouteParam),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/basic/EndpointWithBodyParam",
                httpMethods: new[] { "POST" },
                controller: typeof(BasicController),
                endpoint: nameof(BasicController.EndpointWithBodyParam),
                config: new DeepSleepRequestConfiguration()));

            var configuration = new DeepSleepOasConfigurationProvider
            {
                Info = new OpenApiInfo
                {
                    Description = "Test",
                    Title = "Test"
                },
                PrefixNamesWithNamespace = false
            };

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.Is<Type>(t => t == typeof(IDeepSleepOasConfigurationProvider)))).Returns(configuration);
            mockServiceProvider.Setup(m => m.GetService(It.Is<Type>(t => t == typeof(IApiRoutingTable)))).Returns(table);

            var generator = new DeepSleepOasGenerator(mockServiceProvider.Object);

            var document = await generator.Generate().ConfigureAwait(false);
            
            
            var resultsJsonV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);
            var resultsJsonV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
            var resultsYamlV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Yaml);
            var resultsYamlV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Yaml);
        }

        [Fact]
        public async Task openapi___generates_v2_v3_for_json_and_yaml_for_lists()
        {
            var table = new ApiRoutingTable();
            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/list",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.List),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/list1",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.List1),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/list2",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.List2),
                config: new DeepSleepRequestConfiguration()));

            table.AddRoute(new DeepSleepRouteRegistration(
                template: "/test/list/container",
                httpMethods: new[] { "GET" },
                controller: typeof(ListController),
                endpoint: nameof(ListController.ListContainer),
                config: new DeepSleepRequestConfiguration()));

            var configuration = new DeepSleepOasConfigurationProvider
            {
                Info = new OpenApiInfo
                {
                    Description = "Test",
                    Title = "Test"
                },
                PrefixNamesWithNamespace = false
            };

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(m => m.GetService(It.Is<Type>(t => t == typeof(IDeepSleepOasConfigurationProvider)))).Returns(configuration);
            mockServiceProvider.Setup(m => m.GetService(It.Is<Type>(t => t == typeof(IApiRoutingTable)))).Returns(table);

            var generator = new DeepSleepOasGenerator(mockServiceProvider.Object);
            var document = await generator.Generate().ConfigureAwait(false);

            var resultsJsonV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);
            var resultsJsonV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Json);
            var resultsYamlV2 = document.Serialize(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Yaml);
            var resultsYamlV3 = document.Serialize(OpenApiSpecVersion.OpenApi3_0, OpenApiFormat.Yaml);
        }
    }
}
