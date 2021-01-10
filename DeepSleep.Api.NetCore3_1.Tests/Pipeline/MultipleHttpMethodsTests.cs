namespace DeepSleep.Api.NetCore.Tests.Pipeline
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using DeepSleep.Validation;

    public class MultipleHttpMethodsTests : PipelineTestBase
    {
        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task multimethods___should_return_for_configured_methods(string method)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/pipeline/multiple/methods HTTP/1.1
Host: {host}
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
                expectedContentLength: 16,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task multimethods___should_return_auto_head_for_configured_methods()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/pipeline/multiple/methods HTTP/1.1
Host: {host}
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
                expectedContentLength: 16,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task multimethods___should_return_for_configured_no_auto_head_methods(string method)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/pipeline/multiple/methods/no/auto/head HTTP/1.1
Host: {host}
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
                expectedContentLength: 16,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task multimethods___should_return_not_allow_head_for_configured_no_auto_head_methods()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/pipeline/multiple/methods/no/auto/head HTTP/1.1
Host: {host}
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

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
                    { "X-CorrelationId", $"{correlationId}" },
                    { "Allow", "GET, PUT, POST" }
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task multimethods___should_return_for_configured_methods_with_request_body(string method)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/pipeline/multiple/methods HTTP/1.1
Host: {host}
Accept: {applicationJson}
X-CorrelationId: {correlationId}
Content-Type: {applicationJson}

{{
    ""Value"": ""MyValue""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                expectedContentLength: 19,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("MyValue");
        }
    }
}
