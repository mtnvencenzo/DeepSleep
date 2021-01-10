namespace DeepSleep.Api.NetCore.Tests.Pipeline
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using DeepSleep.Validation;

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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" },
                    { "X-Allow-Content-Types", "application/xml, other/xml" }
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
                extendedHeaders: new NameValuePairs<string, string>
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}" },
                    { "X-Allow-Content-Types", "other/xml" }
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
                extendedHeaders: new NameValuePairs<string, string>
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

        [Fact]
        public async Task readerresolver___should_allow_plain_text()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/plaintext HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textPlain}
Content-Type: {textPlain}
X-CorrelationId: {correlationId}
X-PrettyPrint: true

This is my text data";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textPlain,
                expectedPrettyPrint: true,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("This is my text data");
        }

        [Fact]
        public async Task readerresolver___shouldnt_allow_anything_but_plain_text()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/plaintext HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textJson}
Content-Type: {textJson}
X-CorrelationId: {correlationId}

This is my text data";

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
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-Allow-Accept", "text/plain" }
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task readerresolver___should_allow_json_when_formatters_extended_with_plain_text()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/all/plus/plaintext HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textJson}
Content-Type: {textJson}
X-CorrelationId: {correlationId}

""This is my text data""";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: textJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("This is my text data");
        }

        [Fact]
        public async Task readerresolver___should_allow_xml_when_formatters_extended_with_plain_text()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/all/plus/plaintext HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {applicationXml}
X-CorrelationId: {correlationId}

<string>This is my text data</string>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("This is my text data");
        }
        [Fact]
        public async Task readerresolver___should_allow_plain_text_when_formatters_extended_with_plain_text()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readresolver/all/plus/plaintext HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textPlain}
Content-Type: {textPlain}
X-CorrelationId: {correlationId}

This is my text data";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: textPlain,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("This is my text data");
        }

    }
}


