namespace DeepSleep.Api.NetCore.Tests.Discovery
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.Auth;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Discovery;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AttributeDiscovery_LanguageSupportTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___languagesupport_defaults_correctly()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/languagesupport/default HTTP/1.1
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
        public async Task discovery_attribute___languagesupport_fallback_deDe_no_accept_language()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/languagesupport/fallaback/de-DE HTTP/1.1
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
                expectedCulture: "de-DE",
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___languagesupport_fallback_deDe_accept_language_not_matching()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/languagesupport/fallaback/de-DE HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept-Language: en-US; q=1, es-ES; q=0.9
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
                expectedCulture: "de-DE",
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___languagesupport_fallback_en_accept_language_matching()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/languagesupport/fallaback/en HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept-Language: en; q=1, es-ES; q=0.9
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
                expectedCulture: "en",
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___languagesupport_fallback_en_accept_language_notmatching_supported()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/languagesupport/fallaback/en/with/supported HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept-Language: en; q=1, en-US; q=0.1, es-ES; q=0.9
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
                expectedCulture: "es-ES",
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }
    }
}
