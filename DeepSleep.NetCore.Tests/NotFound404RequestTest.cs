namespace DeepSleep.NetCore.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using System.Threading.Tasks;
    using DeepSleep.NetCore.Tests.Mocks;
    using System;
    using System.Collections.Generic;

    public class NotFound404RequestTest : PipelineTestBase
    {
        private int endpointInvocationCount = 0;

        public NotFound404RequestTest()
            : base()
        {
        }

        [Theory]
        [InlineData("/test/widgets")]
        [InlineData("/tests/widget")]
        [InlineData("/test")]
        [InlineData("/test/widget/other")]
        [InlineData("/test/widget/1")]
        public async Task not_found_route_should_return_404(string relativeUri)
        {
            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}{relativeUri} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: */*
Referer: https://www.google.com/
Accept-Language: en-US,en;q=0.9
X-CorrelationId: {correlationId}";


            var routingTable = this.serviceProvider.GetService<IApiRoutingTable>();
            routingTable.AddRoute("GET_test/widget", "test/widget", "GET", this.GetType(), nameof(GetWidgetEndpoint));

            using (var httpContext = new MockHttpContext(this.serviceProvider, request))
            {
                var apiContext = await Invoke(httpContext).ConfigureAwait(false);
                var response = httpContext.Response;

                Assert.Equal(0, this.endpointInvocationCount);
                Assert.NotNull(response);
                Assert.NotNull(apiContext?.ResponseInfo?.ResponseObject);

                Assert.Equal(404, response.StatusCode);
                Assert.Equal(404, apiContext.ResponseInfo.ResponseObject.StatusCode);
                Assert.Equal(ApiValidationState.NotAttempted, apiContext.ValidationState());

                // Check for headers
                Assert.Equal(5, response.Headers.Count);
                Assert.True(response.Headers.ContainsKey("Date"));
                Assert.True(response.Headers.ContainsKey("X-CorrelationId"));
                Assert.True(response.Headers.ContainsKey("Cache-Control"));
                Assert.True(response.Headers.ContainsKey("Expires"));
                Assert.True(response.Headers.ContainsKey("Content-Length"));

                Assert.Equal(correlationId.ToString(), response.Headers["X-CorrelationId"]);
            }
        }

        private Task<ApiResponse> GetWidgetEndpoint()
        {
            this.endpointInvocationCount++;

            return Task.FromResult(new ApiResponse
            {
                StatusCode = 200,
                Body = new ApiResult
                {
                    StatusCode = 200,
                    Messages = new List<ApiResponseMessage>
                    {
                        new ApiResponseMessage
                        {
                            Code = "200",
                            Message = "Success"
                        }
                    }
                }
            });
        }
    }
}
