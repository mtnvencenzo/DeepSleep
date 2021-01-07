namespace DeepSleep.Api.NetCore.Tests.ValidationErrors
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ValidationErrorsTests : PipelineTestBase
    {
        [Fact]
        public async Task validationerrors___uses_ilist_error_response_json()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validationerrors/get HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 400,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<List<string>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(1);
            data[0].Should().Be("Test Error");
        }
        [Fact]
        public async Task validationerrors___uses_ilist_error_response_xml()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validationerrors/get HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationXml}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 400,
                shouldHaveResponse: true,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<List<string>>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().HaveCount(1);
            data[0].Should().Be("Test Error");
        }
    }
}
