namespace DeepSleep.Api.Web.Tests
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ContextDumpTests : PipelineTestBase
    {
        [Fact]
        public async Task context___get_dump()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/context/dump HTTP/1.1
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
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = apiContext.Dump();
            data.Should().NotBeNullOrWhiteSpace();

            System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
        }

        [Fact]
        public async Task context___post_dump()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/context/dump HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Content-Type: application/json
Accept: {applicationJson}
X-CorrelationId: {correlationId}

{{
    ""Value"": ""Test""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = apiContext.Dump();
            data.Should().NotBeNullOrWhiteSpace();

            System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
        }
    }
}