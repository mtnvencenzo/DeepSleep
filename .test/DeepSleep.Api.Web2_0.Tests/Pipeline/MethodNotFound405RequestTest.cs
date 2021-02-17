namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class MethodNotFound405RequestTest : PipelineTestBase
    {
        [Theory]
        [InlineData("POST")]
        [InlineData("TRACE")]
        [InlineData("DELETE")]
        [InlineData("OPTIONS")]
        [InlineData("PATCH")]
        public async Task method_not_found_for_get_route_with_enabled_head_should_return_405(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method} https://{host}/method/not/found HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: */* 
Referer: https://www.google.com/
Accept-Language: en-US,en;q=0.9
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow", "GET, PUT, HEAD" }
                });

            var data = await base.GetResponseData<MethodNotFoundModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("TRACE")]
        [InlineData("DELETE")]
        [InlineData("OPTIONS")]
        [InlineData("PATCH")]
        [InlineData("HEAD")]
        public async Task method_not_found_for_get_route_with_no_head_should_return_405(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method} https://{host}/method/not/found/nohead HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: */* 
Referer: https://www.google.com/
Accept-Language: en-US,en;q=0.9
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow", "GET, PUT" }
                });

            var data = await base.GetResponseData<MethodNotFoundModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
