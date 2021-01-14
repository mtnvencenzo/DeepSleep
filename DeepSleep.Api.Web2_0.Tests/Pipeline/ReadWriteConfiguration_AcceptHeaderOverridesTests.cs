namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ReadWriteConfiguration_AcceptHeaderOverridesTests : PipelineTestBase
    {
        [Theory]
        [InlineData("text/json")]
        [InlineData("application/json")]
        [InlineData("application/json; q=1, application/xml; q=0, text/xml; q=0")]
        public async Task acceptheaderoverride___should_be_xml(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/acceptheaderoverride/xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textXml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(accept);
            data.AcceptHeaderOverride.Should().Be("text/xml");
        }

        [Fact]
        public async Task acceptheaderoverride___null_request_header_should_be_xml()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/acceptheaderoverride/xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textXml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be("text/xml");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task acceptheaderoverride___empty_request_header_should_be_xml(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/acceptheaderoverride/xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textXml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be("text/xml");
        }

        [Fact]
        public async Task acceptheaderoverride___not_acceptable_should_be_406()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/acceptheaderoverride/406 HTTP/1.1
Host: {host}
Connection: keep-alive
Accept: {applicationJson}
User-Agent: UnitTest/1.0 DEV
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" },
                    { "X-Allow-Accept", $"application/json, text/json, text/xml, application/xml" }
                });
        }
    }
}