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
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class BodyBinding_RequestDeserializationErrorsTests : PipelineTestBase
    {
        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        public async Task body_binding___returns_450_badrequestformat_for_json_invalid_request(string method)
        {
            base.SetupEnvironment(services =>
            {
            });


            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/binding/body/bad/request/format HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}
X-CorrelationId: {correlationId}
Content-Length: 1

{{
    ""Value"": ""This is way too long of a request""
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 450,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        public async Task body_binding___returns_450_badrequestformat_for_xml_invalid_request(string method)
        {
            base.SetupEnvironment(services =>
            {
            });


            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/binding/body/bad/request/format HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {applicationXml}
X-CorrelationId: {correlationId}
Content-Length: 1

<MaxRequestLengthModel>
    <Value>This is way too long of a request</Value>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 450,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Theory]
        [InlineData("POST")]
        [InlineData("PUT")]
        [InlineData("PATCH")]
        public async Task body_binding___returns_450_badrequestformat_for_multipart_invalid_request(string method)
        {
            base.SetupEnvironment(services =>
            {
            });


            var correlationId = Guid.NewGuid();
            var request = @$"
{method} https://{host}/binding/body/bad/request/format HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {multipartFormData}
X-CorrelationId: {correlationId}

--{multipartBoundary}
Content-Disposition: form-data; name=""Value""

test data
--{multipartBoundary}";
// missing final boundary terminator '--' to make this fail

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 450,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }
    }
}
