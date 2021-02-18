namespace DeepSleep.Api.Web.Tests.Binding
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class BodyBinding_ContentTypesTests : PipelineTestBase
    {
        [Theory]
        [InlineData("post")]
        [InlineData("put")]
        [InlineData("patch")]
        public async Task body_binding___charset_utf8_json_simple_post(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method.ToUpper()} https://{host}/binding/simple/{method} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}; charset=utf-8

{{
    ""Value"": ""This is my request""
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
                    
                });

            var data = await base.GetResponseData<MaxRequestLengthModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("This is my request");
        }

        [Theory]
        [InlineData("post")]
        [InlineData("put")]
        [InlineData("patch")]
        public async Task body_binding___charset_usascii_json_simple_post(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method.ToUpper()} https://{host}/binding/simple/{method} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {applicationJson}; charset=us-ascii

{{
    ""Value"": ""This is my request""
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
                    
                });

            var data = await base.GetResponseData<MaxRequestLengthModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("This is my request");
        }

        [Theory]
        [InlineData("post")]
        [InlineData("put")]
        [InlineData("patch")]
        public async Task body_binding___charset_utf8_xml_simple(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method.ToUpper()} https://{host}/binding/simple/{method} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {applicationXml}; charset=utf-8

<MaxRequestLengthModel>
    <Value>This is my request</Value>
</MaxRequestLengthModel>";

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

            var data = await base.GetResponseData<MaxRequestLengthModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("This is my request");
        }

        [Theory]
        [InlineData("post")]
        [InlineData("put")]
        [InlineData("patch")]
        public async Task body_binding___charset_usascii_xml_simple(string method)
        {
            base.SetupEnvironment();


            var request = @$"
{method.ToUpper()} https://{host}/binding/simple/{method} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}
Content-Type: {applicationXml}; charset=us-ascii

<MaxRequestLengthModel>
    <Value>This is my request</Value>
</MaxRequestLengthModel>";

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

            var data = await base.GetResponseData<MaxRequestLengthModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("This is my request");
        }

        [Fact]
        public async Task body_binding___charset_utf8_multipart_simple()
        {
            base.SetupEnvironment();



            var multipart = $@"--{multipartBoundary}
Content-Disposition: form-data; name=""Value""

This is my request
--{multipartBoundary}--
";

            var request = $@"
POST https://{host}/binding/simple/multipart HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {multipartFormData}; charset=utf-8

{multipart.Replace(System.Environment.NewLine, "\r\n")}";

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
                });

            var data = await base.GetResponseData<SimpleMultipartRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("This is my request");
        }

        [Fact]
        public async Task body_binding___chartset_usascii_multipart_simple()
        {
            base.SetupEnvironment();



            var multipart = $@"--{multipartBoundary}
Content-Disposition: form-data; name=""Value""

This is my request
--{multipartBoundary}--
";

            var request = @$"
POST https://{host}/binding/simple/multipart HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Content-Type: {multipartFormData}; charset=""us-ascii""

{multipart.Replace(System.Environment.NewLine, "\r\n")}";

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
                    
                });

            var data = await base.GetResponseData<SimpleMultipartRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("This is my request");
        }
    }
}
