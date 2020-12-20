namespace DeepSleep.Api.NetCore.Tests.Binding
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using global::Api.DeepSleep.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class BodyBinding_MaxRequestLengthTests : PipelineTestBase
    {
        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        public async Task body_binding___returns_413_payloadtoolarge_for_post_exceeding_configured_max(string method)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}
X-CorrelationId: {correlationId}

{{
    ""Value"": ""This is way too long of a request""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 413,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task body_binding___returns_413_payloadtoolarge_for_json_deserialized_exceeding_configured_max()
        {
            base.SetupEnvironment(services =>
            {
                var jsonFormattingConfiguration = new JsonFormattingConfiguration();

                var instanceMock = new Mock<JsonHttpFormatter>(jsonFormattingConfiguration) { CallBase = true };
                instanceMock
                    .Setup(m => m.ReadType(It.IsAny<Stream>(), It.IsAny<Type>(), It.IsAny<IFormatStreamOptions>()))
                    .Throws(new MockBadHttpRequestException());

                services.RemoveAll(typeof(IFormatStreamReaderWriter));
                services.AddScoped(typeof(IFormatStreamReaderWriter), (p) => instanceMock.Object);
            });


            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}
X-CorrelationId: {correlationId}
Content-Length: 1

{{
    ""Value"": ""This is way too long of a request""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 413,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task body_binding___returns_413_payloadtoolarge_for_xml_deserialized_exceeding_configured_max()
        {
            base.SetupEnvironment(services =>
            {
                var jsonFormattingConfiguration = new JsonFormattingConfiguration();

                var instanceMock = new Mock<XmlHttpFormatter> { CallBase = true };
                instanceMock
                    .Setup(m => m.ReadType(It.IsAny<Stream>(), It.IsAny<Type>(), It.IsAny<IFormatStreamOptions>()))
                    .Throws(new MockBadHttpRequestException());

                services.RemoveAll(typeof(IFormatStreamReaderWriter));
                services.AddScoped(typeof(IFormatStreamReaderWriter), (p) => instanceMock.Object);
            });


            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {applicationXml}
X-CorrelationId: {correlationId}
Content-Length: 1

<MaxRequestLengthModel>
    <Value>This is way too long of a request</Value>
</MaxRequestLengthModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 413,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task body_binding___returns_413_payloadtoolarge_for_multipart_deserialized_exceeding_configured_max()
        {
            base.SetupEnvironment(services =>
            {
                var multipartStreamReaderMock = new Mock<IMultipartStreamReader>();
                multipartStreamReaderMock.Setup(m => m.ReadAsMultipart(It.IsAny<Stream>()))
                    .Throws(new MockBadHttpRequestException());

                var instanceMock = new Mock<MultipartFormDataFormatter>(multipartStreamReaderMock.Object, null) { CallBase = true };
                instanceMock
                    .Setup(m => m.ReadType(It.IsAny<Stream>(), It.IsAny<Type>(), It.IsAny<IFormatStreamOptions>()))
                    .Throws(new MockBadHttpRequestException());

                services.RemoveAll(typeof(IFormatStreamReaderWriter));
                services.AddScoped(typeof(IFormatStreamReaderWriter), (p) => instanceMock.Object);
                services.AddScoped(typeof(IFormatStreamReaderWriter), (p) => new XmlHttpFormatter());
            });


            var correlationId = Guid.NewGuid();

            var multipart = $@"
--{multipartBoundary}
Content-Disposition: form-data; name=""Value""

test data
--{multipartBoundary}--
";

            var request = @$"
POST https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {multipartFormData}
X-CorrelationId: {correlationId}
Content-Length: 1

{multipart.Replace(Environment.NewLine, "\r\n")}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 413,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }
    }
}
