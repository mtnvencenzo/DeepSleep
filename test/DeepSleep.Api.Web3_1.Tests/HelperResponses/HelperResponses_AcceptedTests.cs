﻿namespace DeepSleep.Api.Web.Tests.HelperResponses
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.HelperResponses;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class HelperResponses_AcceptedTests : PipelineTestBase
    {
        [Fact]
        public async Task accepted___has_response()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/helper/responses/accepted HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 202,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<HelperResponseModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task accepted___null_response()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/helper/responses/accepted/null HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 202,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Location", "/test/location" }
                });

            var data = await base.GetResponseData<HelperResponseModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task accepted___has_response_with_headers()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/helper/responses/accepted/headers HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 202,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Test", "Value"},
                    { "Location", "/test/location" }
                });

            var data = await base.GetResponseData<HelperResponseModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
