﻿namespace DeepSleep.Api.Web.Tests.Binding
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class BodyBinding_MultipartFormDataTests : PipelineTestBase
    {
        [Fact]
        public async Task body_binding___multipart_binds_to_multipart_object()
        {
            base.SetupEnvironment();



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
                    
                });

            var data = await base.GetResponseData<SimpleMultipartRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("test data");
            data.OtherValue.Should().Be("other test data");

            var shouldBe = @$"This is a bunch of text data on multiple lines";

            data.TextFileData.Should().Be(shouldBe.Replace(Environment.NewLine, "\r\n"));
        }

        [Fact]
        public async Task body_binding___multipart_binds_to_custom_object()
        {
            base.SetupEnvironment();



            var multipart = $@"
--{multipartBoundary}
Content-Disposition: form-data; name=""value""

test data
--{multipartBoundary}
Content-Disposition: form-data; name=""otherValue""

other test data
--{multipartBoundary}
Content-Disposition: form-data; name=""files""; filename=""Test.txt""
Content-Type: text/plain; charset=utf-8

This is a bunch of text data on multiple lines
--{multipartBoundary}--
";

            var request = @$"
POST https://{host}/binding/multipart/custom HTTP/1.1
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
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
