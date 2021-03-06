﻿namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class NotFound404RequestTest : PipelineTestBase
    {
        [Theory]
        [InlineData("/route/not/founds")]
        [InlineData("/routes/not/found")]
        [InlineData("/route")]
        [InlineData("/route/not/found/something")]
        [InlineData("/route/not/found/1")]
        [InlineData("/routenot/found")]
        [InlineData("/route/nots/found")]
        public async Task not_found_route_should_return_404(string relativeUri)
        {
            // uses template 'route/not/found'
            base.SetupEnvironment();


            var request = @$"
GET https://{host}{relativeUri} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: */*
Referer: https://www.google.com/
Accept-Language: en-US,en;q=0.9
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 404,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });
        }

        [Theory]
        [InlineData("/route/not/founds")]
        [InlineData("/routes/not/found")]
        [InlineData("/route")]
        [InlineData("/route/not/found/something")]
        [InlineData("/route/not/found/1")]
        [InlineData("/routenot/found")]
        [InlineData("/route/nots/found")]
        public async Task not_found_route_for_enabled_head_should_return_404(string relativeUri)
        {
            // uses template 'route/not/found'
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}{relativeUri} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: */*
Referer: https://www.google.com/
Accept-Language: en-US,en;q=0.9
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 404,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });
        }
    }
}
