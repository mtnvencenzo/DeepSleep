namespace DeepSleep.Api.Web.Tests.Authentication
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Authentication;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class Authentication_AnonymousTests : PipelineTestBase
    {
        // Anonymous Access
        // -------------------------------

        [Fact]
        public async Task authentication___anonymous_with_valid_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authentication___anonymous_with_invalid_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token sddsdsdsd
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authentication___anonymous_with_empty_header_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization:
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authentication___anonymous_with_scheme_only_header_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authentication___anonymous_with_no_authentication_header()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/authentication/anonymous/allowed HTTP/1.1
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }


        [Fact]
        public async Task authentication___anonymous_head_with_valid_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authentication___anonymous_head_with_invalid_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token sddsdsdsd
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authentication___anonymous_head_with_empty_header_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization:
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authentication___anonymous_head_with_scheme_only_header_authentication()
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/authentication/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authentication___anonymous_head_with_no_authentication_header()
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/authentication/anonymous/allowed HTTP/1.1
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
