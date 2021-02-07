namespace DeepSleep.Api.Web.Tests.Authentication
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Authentication;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class Authorization_AnonymousTests : PipelineTestBase
    {
        // Anonymous Access
        // -------------------------------

        [Fact]
        public async Task authorization___anonymous_with_default_configuration()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authorization___anonymous_with_configured_policy_failing_auth_provider()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token sddsdsdsd
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authorization___anonymous_with_configured_policy_success_auth_provider()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization:
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authorization___anonymous_with_configured_policy_not_matching_any_provider()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task authorization___anonymous_head_with_default_configuration()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authorization___anonymous_head_with_configured_policy_failing_auth_provider()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token sddsdsdsd
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authorization___anonymous_head_with_configured_policy_success_auth_provider()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization:
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task authorization___anonymous_head_with_configured_policy_not_matching_any_provider()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/authorization/anonymous/allowed HTTP/1.1
Host: {host}
Authorization: Token
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
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: Auth.AuthenticationType.Anonymous,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: Auth.AuthorizationType.Anonymous,
                expectedContentLength: 63,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
