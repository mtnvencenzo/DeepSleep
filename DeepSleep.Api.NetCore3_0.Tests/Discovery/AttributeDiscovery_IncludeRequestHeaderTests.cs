namespace DeepSleep.Api.NetCore.Tests.Discovery
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.Auth;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Discovery;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using DeepSleep.Validation;

    public class AttributeDiscovery_IncludeRequestHeaderTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___includerequestheader_defaults_correctly()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/includeRequestHeader/default HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 16,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___includerequestheader_true()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/includeRequestHeader/true HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 16,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___includerequestheader_false()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/includeRequestHeader/false HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 16,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectRequestIdHeader: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }
    }
}
