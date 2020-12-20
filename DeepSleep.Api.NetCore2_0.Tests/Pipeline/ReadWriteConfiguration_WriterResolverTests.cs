namespace DeepSleep.Api.NetCore.Tests.Pipeline
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ReadWriteConfiguration_WriterResolverTests : PipelineTestBase
    {
        [Theory]
        [InlineData("text/json")]
        [InlineData("text/json, application/xml; q=0, application/json, other/xml; q=0, text/xml")]
        [InlineData("text/xml")]
        public async Task writerresolver___should_reject_unsupported_types(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeresolver/text-xml HTTP/1.1
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
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" },
                    { "X-Allow-Accept", "application/xml, other/xml" }
                });
        }

        [Theory]
        [InlineData("other/xml", "other/xml")]
        [InlineData("application/xml", "application/xml")]
        [InlineData("text/xml, application/xml; q=0, other/xml; q=0.1", "other/xml")]
        [InlineData("text/xml, other/xml; q=0.1, application/xml; q=0.2", "application/xml")]
        public async Task writerresolver___should_allow_supported_types(string accept, string expectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeresolver/text-xml HTTP/1.1
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
                expectedContentType: expectedContentType,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(accept);
            data.AcceptHeaderOverride.Should().BeNull();
            data.ReadableTypes.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be(null);
            data.WriteableTypes.Should().BeNull();
        }

        [Theory]
        [InlineData("text/json")]
        [InlineData("text/json, application/xml; q=0, application/json, other/xml; q=0, text/xml")]
        [InlineData("text/xml")]
        [InlineData("other/xml")]
        [InlineData("application/xml")]
        [InlineData("text/xml, application/xml; q=0, other/xml; q=0.1")]
        [InlineData("text/xml, other/xml; q=0.1, application/xml; q=0.2")]
        public async Task writerresolver___should_always_reject_when_writerresolver_retunrs_no_formatters(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeresolver/none HTTP/1.1
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
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" },
                    { "X-Allow-Accept", string.Empty }
                });
        }

        [Theory]
        [InlineData("text/json")]
        [InlineData("text/xml")]
        [InlineData("application/xml")]
        [InlineData("text/json, application/xml; q=1, application/json, other/xml; q=0, text/xml")]
        [InlineData("text/xml, application/xml")]
        public async Task writerresolver___should_reject_unsupported_writeable_types(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeresolver/writeabletypes HTTP/1.1
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
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" },
                    { "X-Allow-Accept", "other/xml" }
                });
        }


        [Theory]
        [InlineData("other/xml", "other/xml")]
        [InlineData("application/xml, other/xml", "other/xml")]
        [InlineData("text/xml, application/xml; q=0, other/xml; q=0.1", "other/xml")]
        [InlineData("text/xml, other/xml; q=0.1, application/xml; q=0.2", "other/xml")]
        public async Task writerresolver___should_allow_supported_writeable_types(string accept, string expectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeresolver/writeabletypes HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}
X-PrettyPrint: true";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: expectedContentType,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedPrettyPrint: true,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-PrettyPrint", "true"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(accept);
            data.AcceptHeaderOverride.Should().BeNull();
            data.ReadableTypes.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be(null);
            data.WriteableTypes.Should().Be("other/xml,text/xml");
        }
    }
}


