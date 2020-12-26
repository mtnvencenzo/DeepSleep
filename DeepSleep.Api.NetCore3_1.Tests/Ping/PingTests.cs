namespace DeepSleep.Api.NetCore.Tests.Ping
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.NetCore;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class PingTests : PipelineTestBase
    {
        [Fact]
        public async Task ping___json_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/ping HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("Pong");
        }

        [Fact]
        public async Task ping___xml_success()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/ping HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationXml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("Pong");
        }

        [Fact]
        public void ping___adds_ping_route_by_default()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration();
            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "ping");

            Assert.NotNull(route);
        }

        [Fact]
        public void ping___does_not_add_ping_route()
        {
            IServiceCollection services = new ServiceCollection();

            var config = new DefaultApiServiceConfiguration
            {
                UsePingEndpoint = false
            };

            services.UseApiCoreServices(config);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider);

            var table = provider.GetService<IApiRoutingTable>();
            Assert.NotNull(table);

            var routes = table.GetRoutes().ToList();
            var route = routes.FirstOrDefault(r => r.Template == "ping");

            Assert.Null(route);
        }
    }
}
