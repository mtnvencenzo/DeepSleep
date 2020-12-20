namespace DeepSleep.Api.NetCore.Tests.Pipeline
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ReadWriteConfiguration_ReaderResolverTests : PipelineTestBase
    {
        [Theory]
        [InlineData("text/json")]
        [InlineData("text/xml")]
        [InlineData("application/json")]
        [InlineData("something/xml")]
        public async Task readerresolver___should_reject_unsupported_types(string contentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: application/json
Content-Type: {contentType}
X-CorrelationId: {correlationId}

<ReadWriteOverrideModel>
    <AcceptHeader>test</AcceptHeader>
</ReadWriteOverrideModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 415,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" }
                });
        }

        [Theory]
        [InlineData("application/xml")]
        [InlineData("other/xml")]
        public async Task readerresolver___should_allow_supported_types(string contentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textJson}
Content-Type: {contentType}
X-CorrelationId: {correlationId}
X-PrettyPrint: true

<ReadWriteOverrideModel>
    <AcceptHeader>test</AcceptHeader>
</ReadWriteOverrideModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textJson,
                expectedPrettyPrint: true,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-PrettyPrint", "true"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(textJson);
            data.AcceptHeaderOverride.Should().BeNull();
            data.WriteableTypes.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be(null);
            data.ReadableTypes.Should().BeNull();
        }

        [Theory]
        [InlineData("text/json")]
        [InlineData("text/xml")]
        [InlineData("application/json")]
        [InlineData("something/xml")]
        [InlineData("application/xml")]
        public async Task readerresolver___should_reject_unsupported_readable_types(string contentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/readabletypes HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textJson}
Content-Type: {contentType}
X-CorrelationId: {correlationId}

<ReadWriteOverrideModel>
    <AcceptHeader>test</AcceptHeader>
</ReadWriteOverrideModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 415,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" }
                });
        }

        [Theory]
        [InlineData("other/xml")]
        public async Task readerresolver___should_allow_supported_readable_types(string contentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/readabletypes HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textJson}
Content-Type: {contentType}
X-CorrelationId: {correlationId}
X-PrettyPrint: true

<ReadWriteOverrideModel>
    <AcceptHeader>test</AcceptHeader>
</ReadWriteOverrideModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textJson,
                expectedPrettyPrint: true,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-PrettyPrint", "true"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(textJson);
            data.AcceptHeaderOverride.Should().BeNull();
            data.WriteableTypes.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be(null);
            data.ReadableTypes.Should().Be("other/xml");
        }
    }
}


