namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class MultipleHttpMethodsTests : PipelineTestBase
    {
        [Theory]
        [InlineData("GET")]
        [InlineData("POST")]
        [InlineData("PUT")]
        public async Task multimethods___should_return_for_configured_methods(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method} https://{host}/pipeline/multiple/methods HTTP/1.1
Host: {host}
Accept: {applicationJson}
";

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
                    
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task multimethods___should_return_auto_head_for_configured_methods()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/pipeline/multiple/methods HTTP/1.1
Host: {host}
Accept: {applicationJson}
";

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


            var request = @$"
{method} https://{host}/pipeline/multiple/methods/no/auto/head HTTP/1.1
Host: {host}
Accept: {applicationJson}
";

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
                    
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task multimethods___should_return_not_allow_head_for_configured_no_auto_head_methods()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/pipeline/multiple/methods/no/auto/head HTTP/1.1
Host: {host}
Accept: {applicationJson}";

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

            var request = @$"
{method} https://{host}/pipeline/multiple/methods HTTP/1.1
Host: {host}
Accept: {applicationJson}
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
                    
                });

            var data = await base.GetResponseData<MultiMethodsModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("MyValue");
        }
    }
}
