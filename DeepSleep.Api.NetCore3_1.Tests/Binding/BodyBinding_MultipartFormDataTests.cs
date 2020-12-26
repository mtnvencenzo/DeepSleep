namespace DeepSleep.Api.NetCore.Tests.Binding
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.Formatting;
    using DeepSleep.Formatting.Formatters;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using global::Api.DeepSleep.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Xunit;

    public class BodyBinding_MultipartFormDataTests : PipelineTestBase
    {
        [Fact]
        public async Task body_binding___multipart_binds_to_multipart_object()
        {
            base.SetupEnvironment(services =>
            {
            });


            var correlationId = Guid.NewGuid();

            var multipart = $@"
--{multipartBoundary}
Content-Disposition: form-data; name=""Value""

test data
--{multipartBoundary}
Content-Disposition: form-data; name=""OtherValue""

other test data
--{multipartBoundary}
Content-Disposition: form-data; name=""Files""; filename=""Test.txt""
Content-Type: text/plain; charset=utf-8

This is a bunch of text data on multiple lines
--{multipartBoundary}--
";

            var request = @$"
POST https://{host}/binding/simple/multipart HTTP/1.1
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleMultipartRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("test data");
            data.OtherValue.Should().Be("other test data");

            var shouldBe = @$"This is a bunch of text data on multiple lines";
            
            data.TextFileData.Should().Be(shouldBe.Replace(Environment.NewLine, "\r\n"));
        }
    }
}
