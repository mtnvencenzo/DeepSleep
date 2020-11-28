namespace DeepSleep.NetCore.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using System.Threading.Tasks;
    using DeepSleep.NetCore.Tests.Mocks;
    using System;
    using System.Collections.Generic;

    public class MethodNotFound405RequestTest : PipelineTestBase
    {
        private int endpointInvocationCount = 0;

        public MethodNotFound405RequestTest()
            : base()
        {
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("TRACE")]
        [InlineData("DELETE")]
        [InlineData("OPTIONS")]
        [InlineData("PATCH")]
        public async Task method_not_found_for_get_route_should_return_405(string method)
        {
            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/test/widget HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: */* nz 
Referer: https://www.google.com/
Accept-Language: en-US,en;q=0.9
X-CorrelationId: {correlationId}";

            var routingTable = this.serviceProvider.GetService<IApiRoutingTable>();
            routingTable.AddRoute("GET_test/widget", "test/widget", "GET", this.GetType(), nameof(GetWidgetEndpoint));
            routingTable.AddRoute("PUT_test/widget", "test/widget", "PUT", this.GetType(), nameof(GetWidgetEndpoint));

            using (var httpContext = new MockHttpContext(this.serviceProvider, request))
            {
                var apiContext = await Invoke(httpContext).ConfigureAwait(false);
                var response = httpContext.Response;

                Assert.Equal(0, this.endpointInvocationCount);
                Assert.NotNull(response);
                Assert.Null(apiContext?.ResponseInfo?.ResponseObject);

                Assert.Equal(405, response.StatusCode);
                Assert.Equal(405, apiContext.ResponseInfo.StatusCode);
                Assert.Equal(ApiValidationState.NotAttempted, apiContext.ValidationState());

                // Check for headers
                Assert.Equal(6, response.Headers.Count);
                Assert.True(response.Headers.ContainsKey("Date"));
                Assert.True(response.Headers.ContainsKey("X-CorrelationId"));
                Assert.True(response.Headers.ContainsKey("Cache-Control"));
                Assert.True(response.Headers.ContainsKey("Expires"));
                Assert.True(response.Headers.ContainsKey("Content-Length"));
                Assert.True(response.Headers.ContainsKey("Allow"));

                Assert.Equal(correlationId.ToString(), response.Headers["X-CorrelationId"]);
                Assert.Equal("GET, PUT, HEAD", response.Headers["Allow"]);
            }
        }

        private Task<int> GetWidgetEndpoint()
        {
            this.endpointInvocationCount++;

            return Task.FromResult(1);
        }
    }
}
