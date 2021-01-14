namespace DeepSleep.Api.Web.Tests.Validation
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Models;
    using System.Threading.Tasks;
    using Xunit;

    public class ValidatorsTests : PipelineTestBase
    {
        [Fact]
        public async Task validators___correctly_handles_continuation()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validators/get HTTP/1.1
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
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(2);
            data.Messages[0].ErrorMessageStr.Should().Be("VALIDATOR-2");
            data.Messages[1].ErrorMessageStr.Should().Be("VALIDATOR-1");
        }

        [Fact]
        public async Task validators___correctly_handles_continuation_with_success_and_failures()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validators/get/with/mixed/success/failures HTTP/1.1
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
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(2);
            data.Messages[0].ErrorMessageStr.Should().Be("VALIDATOR-2");
            data.Messages[1].ErrorMessageStr.Should().Be("VALIDATOR-1");
        }

        [Fact]
        public async Task validators___correctly_handles_continuation_with_success_and_mulit_failures()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validators/get/with/mixed/success/failures/multi HTTP/1.1
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
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(4);
            data.Messages[0].ErrorMessageStr.Should().Be("VALIDATOR-4.1");
            data.Messages[1].ErrorMessageStr.Should().Be("VALIDATOR-4.2");
            data.Messages[2].ErrorMessageStr.Should().Be("VALIDATOR-2");
            data.Messages[3].ErrorMessageStr.Should().Be("VALIDATOR-1");
        }

        [Fact]
        public async Task validators___correctly_handles_suggested_status_code()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validators/get/failure/404 HTTP/1.1
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
                expectedHttpStatus: 404,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("VALIDATOR-5");
        }

        [Fact]
        public async Task validators___correctly_handles_continuation_mixed_configured_and_attributes()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/validators/get/failure/mixed/configured HTTP/1.1
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
                expectedHttpStatus: 404,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(5);
            data.Messages[0].ErrorMessageStr.Should().Be("VALIDATOR-4.1");
            data.Messages[1].ErrorMessageStr.Should().Be("VALIDATOR-4.2");
            data.Messages[2].ErrorMessageStr.Should().Be("VALIDATOR-2");
            data.Messages[3].ErrorMessageStr.Should().Be("VALIDATOR-1");
            data.Messages[4].ErrorMessageStr.Should().Be("VALIDATOR-5");
        }
    }
}
