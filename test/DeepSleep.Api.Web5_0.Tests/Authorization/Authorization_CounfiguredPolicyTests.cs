namespace DeepSleep.Api.Web.Tests.Authentication
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Auth;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Authorization;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class Authorization_CounfiguredPolicyTests : PipelineTestBase
    {
        [Fact]
        public async Task authorization___policy_matches_failing_auth_provider()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/authorization/policy/configured/failing/provider HTTP/1.1
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
                expectedHttpStatus: 403,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthenticationScheme: "Token",
                expectedAuthenticationValue: staticToken,
                expectedAuthorizationResult: false,
                expectedAuthorizedBy: AuthorizationType.Provider,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthorizationModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
        [Fact]
        public async Task authorization___policy_matches_failing_and_success_auth_provider()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/authorization/policy/configured/mixed/providers HTTP/1.1
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
                expectedHttpStatus: 403,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthenticationScheme: "Token",
                expectedAuthenticationValue: staticToken,
                expectedAuthorizationResult: false,
                expectedAuthorizationResultCount: 2,
                expectedAuthorizedBy: AuthorizationType.Provider,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthorizationModel>(response).ConfigureAwait(false);
            data.Should().BeNull();

            apiContext.Request.ClientAuthorizationInfo.AuthResults.Should().HaveCount(2);

        }

        [Fact]
        public async Task authorization___policy_matches_success_auth_provider()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/authorization/policy/configured/success/provider HTTP/1.1
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
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthenticationScheme: "Token",
                expectedAuthenticationValue: staticToken,
                expectedAuthorizationResult: true,
                expectedAuthorizedBy: AuthorizationType.Provider,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthorizationModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AuthorizedBy.Should().Be(AuthorizationType.Provider);
            data.Value.Should().Be("Test");
            data.IsAuthorized.Should().BeTrue();
        }
    }
}
