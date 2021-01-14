namespace DeepSleep.Api.Web.Tests.HelperResponses
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.HelperResponses;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class HelperResponses_CreatedTests : PipelineTestBase
    {
        [Fact]
        public async Task created___has_response()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/helper/responses/created HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 201,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<HelperResponseModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task created___null_response()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/helper/responses/created/null HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 201,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<HelperResponseModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task created___has_response_with_headers()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/helper/responses/created/headers HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 201,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "Test", "Value"}
                });

            var data = await base.GetResponseData<HelperResponseModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }
    }
}
