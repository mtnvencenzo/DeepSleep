namespace DeepSleep.Api.NetCore.Tests.Discovery
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using DeepSleep.Auth;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Discovery;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AttributeDiscovery_CrossOriginTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___crossorigin_preflight_default()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
OPTIONS https://{host}/discovery/attribute/crossorigin/default HTTP/1.1
Host: {host}
Origin: https:www.test.co
Access-Control-Request-Method: GET
Access-Control-Request-Headers: Content-Type
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https:www.test.co" },
                    { "Access-Control-Allow-Methods", "GET, HEAD" },
                    { "Access-Control-Allow-Credentials", "true" },
                    { "Access-Control-Allow-Headers", "Content-Type" },
                    { "Access-Control-Max-Age", "600" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_preflight_default_with_caching()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
OPTIONS https://{host}/discovery/attribute/crossorigin/default/with/caching HTTP/1.1
Host: {host}
Origin: https:www.test.co
Access-Control-Request-Method: GET
Access-Control-Request-Headers: Content-Type
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https:www.test.co" },
                    { "Access-Control-Allow-Methods", "GET, HEAD" },
                    { "Access-Control-Allow-Credentials", "true" },
                    { "Access-Control-Allow-Headers", "Content-Type" },
                    { "Access-Control-Max-Age", "600" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_preflight_specified_empty()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
OPTIONS https://{host}/discovery/attribute/crossorigin/specified/empty HTTP/1.1
Host: {host}
Origin: https:www.test.co
Access-Control-Request-Method: GET
Access-Control-Request-Headers: Content-Type
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "" },
                    { "Access-Control-Allow-Methods", "GET, HEAD" },
                    { "Access-Control-Allow-Credentials", "false" },
                    { "Access-Control-Allow-Headers", "" },
                    { "Access-Control-Max-Age", "100" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_preflight_specified_using_unmatched_values()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
OPTIONS https://{host}/discovery/attribute/crossorigin/specified HTTP/1.1
Host: {host}
Origin: https:www.test.co
Access-Control-Request-Method: GET
Access-Control-Request-Headers: Content-Type
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "" },
                    { "Access-Control-Allow-Methods", "GET, HEAD" },
                    { "Access-Control-Allow-Credentials", "false" },
                    { "Access-Control-Allow-Headers", "Accept" },
                    { "Access-Control-Max-Age", "100" },
                    { "Access-Control-Expose-Headers", "X-RequestId" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_preflight_specified_using_matched_values()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
OPTIONS https://{host}/discovery/attribute/crossorigin/specified HTTP/1.1
Host: {host}
Origin: https://test.us
Access-Control-Request-Method: GET
Access-Control-Request-Headers: Content-Type
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https://test.us" },
                    { "Access-Control-Allow-Methods", "GET, HEAD" },
                    { "Access-Control-Allow-Credentials", "false" },
                    { "Access-Control-Allow-Headers", "Accept" },
                    { "Access-Control-Max-Age", "100" },
                    { "Access-Control-Expose-Headers", "X-RequestId" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_default()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/crossorigin/default HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Origin: https:www.test.co
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
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https:www.test.co" },
                    { "Access-Control-Allow-Credentials", "true" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_default_with_caching()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/crossorigin/default/with/caching HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Origin: https:www.test.co
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
                expectedCacheControlValue: "public, max-age=120",
                expectedExpiresSecondsAdd: 120,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https:www.test.co" },
                    { "Access-Control-Allow-Credentials", "true" },
                    { "Vary", "Origin, Accept, Accept-Encoding, Accept-Language" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_specified_using_unmatched_values()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/crossorigin/specified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Origin: https:www.test.co
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
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "" },
                    { "Access-Control-Allow-Credentials", "false" },
                    { "Access-Control-Expose-Headers", "X-RequestId" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_specified_using_matched_values()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/crossorigin/specified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Origin: https://test.us
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
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https://test.us" },
                    { "Access-Control-Allow-Credentials", "false" },
                    { "Access-Control-Expose-Headers", "X-RequestId" },
                    { "Vary", "Origin" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___crossorigin_specified_with_caching()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/crossorigin/specified/with/caching HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Origin: https://test.us
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
                expectedCacheControlValue: "public, max-age=120",
                expectedExpiresSecondsAdd: 120,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Origin", "https://test.us" },
                    { "Access-Control-Allow-Credentials", "false" },
                    { "Access-Control-Expose-Headers", "X-RequestId" },
                    { "Vary", "Origin, Accept, Accept-Encoding, Accept-Language" },
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }
    }
}
