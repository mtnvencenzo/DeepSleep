namespace DeepSleep.Api.Web.Tests.Discovery
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Auth;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Discovery;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AttributeDiscovery_UseCorrelationIdHeaderTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___usecorrelationidheader_defaults_correctly()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/usecorrelationidheader/default HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
X-CorrelationId: {correlationId}
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
                    { "X-CorrelationId", correlationId.ToString() }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___usecorrelationidheader_true()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/usecorrelationidheader/true HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
X-CorrelationId: {correlationId}
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
                    { "X-CorrelationId", correlationId.ToString() }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___usecorrelationidheader_false()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/usecorrelationidheader/false HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
X-CorrelationId: {correlationId}
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
                expectRequestIdHeader: true,
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
