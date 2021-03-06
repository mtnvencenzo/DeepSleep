﻿namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class EnableHeadConfigurationTests : PipelineTestBase
    {
        [Fact]
        public async Task enablehead___should_return_405_when_disabled_for_get()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/head/configured/disabled HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeFalse();


            base.SetupEnvironment();

            request = @$"
HEAD https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContextHead = new MockHttpContext(this.ServiceProvider, request);
            apiContext = await Invoke(httpContextHead).ConfigureAwait(false);
            response = httpContextHead.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow", "GET" }
                });

            data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task enablehead___should_return_matching_response_when_default_enabled_for_get()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/head/configured/default HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeTrue();


            base.SetupEnvironment();

            request = @$"
HEAD https://{host}/head/configured/default HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContextHead = new MockHttpContext(this.ServiceProvider, request);
            var headApiContext = await Invoke(httpContextHead).ConfigureAwait(false);
            var headResponse = httpContextHead.Response;

            base.AssertResponse(
                apiContext: headApiContext,
                response: headResponse,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedContentLength: apiContext.Response.ContentLength,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            headApiContext.Response.ContentLength.Should().Be(apiContext.Response.ContentLength);
            headApiContext.Response.Headers.Count.Should().Be(apiContext.Response.Headers.Count);

            headResponse.Headers.Count.Should().Be(response.Headers.Count);

            foreach (var headHeader in headApiContext.Response.Headers)
            {
                var resHeader = apiContext.Response.Headers.FirstOrDefault(h => h.Name == headHeader.Name);
                resHeader.Should().NotBeNull();

                if (headHeader.Name != "X-RequestId" && headHeader.Name != "X-CorrelationId" && headHeader.Name != "Date" && headHeader.Name != "Expires")
                {
                    resHeader.Value.Should().Be(headHeader.Value);
                }
                else
                {
                    resHeader.Value.Should().NotBeNullOrWhiteSpace();
                }
            }

            foreach (var resHeader in apiContext.Response.Headers)
            {
                var headHeader = headApiContext.Response.Headers.FirstOrDefault(h => h.Name == resHeader.Name);
                headHeader.Should().NotBeNull();

                if (resHeader.Name != "X-RequestId" && resHeader.Name != "X-CorrelationId" && resHeader.Name != "Date" && resHeader.Name != "Expires")
                {
                    headHeader.Value.Should().Be(resHeader.Value);
                }
                else
                {
                    headHeader.Value.Should().NotBeNullOrWhiteSpace();
                }
            }


            data = await base.GetResponseData<RequestHeadModel>(headResponse).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task enablehead___should_return_response_when_head_explicitly_set_for_get()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/head/explicit HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeFalse();


            base.SetupEnvironment();

            request = @$"
HEAD https://{host}/head/explicit HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContextHead = new MockHttpContext(this.ServiceProvider, request);
            var headApiContext = await Invoke(httpContextHead).ConfigureAwait(false);
            var headResponse = httpContextHead.Response;

            base.AssertResponse(
                apiContext: headApiContext,
                response: headResponse,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedContentLength: 19,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            data = await base.GetResponseData<RequestHeadModel>(headResponse).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task enablehead___should_return_allow_with_head_for_head_enabled_for_get()
        {
            base.SetupEnvironment();


            var request = @$"
PUT https://{host}/head/configured/default HTTP/1.1
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
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow", $"GET, HEAD"}
                });
        }
    }
}
