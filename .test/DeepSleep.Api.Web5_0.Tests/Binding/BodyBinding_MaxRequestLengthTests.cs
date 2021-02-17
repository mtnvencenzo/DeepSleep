namespace DeepSleep.Api.Web.Tests.Binding
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Configuration;
    using DeepSleep.Media;
    using DeepSleep.Media.Serializers;
    using DeepSleep.Validation;
    using global::Api.DeepSleep.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Moq;
    using System;
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
            base.SetupEnvironment();


            var request = @$"
{method} https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}

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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });
        }

        [Fact]
        public async Task body_binding___returns_413_payloadtoolarge_for_json_deserialized_exceeding_configured_max()
        {
            base.SetupEnvironment(services =>
            {
                var configurationMock = new Mock<JsonMediaSerializerConfiguration>();

                var instanceMock = new Mock<DeepSleepJsonMediaSerializer>(configurationMock.Object) { CallBase = true };
                instanceMock
                    .Setup(m => m.ReadType(It.IsAny<Stream>(), It.IsAny<Type>(), It.IsAny<IMediaSerializerOptions>()))
                    .Throws(new MockBadHttpRequestException());

                services.RemoveAll(typeof(IDeepSleepMediaSerializer));
                services.AddScoped(typeof(IDeepSleepMediaSerializer), (p) => instanceMock.Object);
            });



            var request = @$"
POST https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });
        }

        [Fact]
        public async Task body_binding___returns_413_payloadtoolarge_for_xml_deserialized_exceeding_configured_max()
        {
            base.SetupEnvironment(services =>
            {
                var jsonFormattingConfiguration = new JsonMediaSerializerConfiguration();

                var instanceMock = new Mock<DeepSleepXmlMediaSerializer>(new object[] { null }) { CallBase = true };
                instanceMock
                    .Setup(m => m.ReadType(It.IsAny<Stream>(), It.IsAny<Type>(), It.IsAny<IMediaSerializerOptions>()))
                    .Throws(new MockBadHttpRequestException());

                services.RemoveAll(typeof(IDeepSleepMediaSerializer));
                services.AddScoped(typeof(IDeepSleepMediaSerializer), (p) => instanceMock.Object);
            });



            var request = @$"
POST https://{host}/binding/body/max/request/length HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {applicationXml}
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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
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

                var instanceMock = new Mock<DeepSleepMultipartFormDataMediaSerializer>(multipartStreamReaderMock.Object, null) { CallBase = true };
                instanceMock
                    .Setup(m => m.ReadType(It.IsAny<Stream>(), It.IsAny<Type>(), It.IsAny<IMediaSerializerOptions>()))
                    .Throws(new MockBadHttpRequestException());

                services.RemoveAll(typeof(IDeepSleepMediaSerializer));
                services.AddScoped(typeof(IDeepSleepMediaSerializer), (p) => instanceMock.Object);
                services.AddScoped(typeof(IDeepSleepMediaSerializer), (p) => new DeepSleepXmlMediaSerializer(null));
            });




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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });
        }
    }
}
