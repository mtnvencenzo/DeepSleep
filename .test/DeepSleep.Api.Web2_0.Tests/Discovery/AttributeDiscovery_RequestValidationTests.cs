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

    public class AttributeDiscovery_RequestValidationTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___requestvalidation_defaults_correctly()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/default HTTP/1.1
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
        public async Task discovery_attribute___requestvalidation_max_url_length()
        {
            base.SetupEnvironment();

            string toolong = new string('a', 70);
            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified?var={toolong} HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 414,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___requestvalidation_max_header_length()
        {
            base.SetupEnvironment();

            string toolong = new string('a', 101);
            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified HTTP/1.1
Host: {host}
X-Header: {toolong}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 431,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___requestvalidation_max_request_length()
        {
            base.SetupEnvironment();

            string toolong = new string('a', 101);
            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Content-Type: {applicationJson}
Accept: {applicationJson}

{{
    ""Test"": ""{toolong}""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 413,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___requestvalidation_missing_content_length_not_required()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}

{{
    ""Test"": ""Abc""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request, false);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
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
        public async Task discovery_attribute___requestvalidation_missing_content_length_required()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified/require/contentlength HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}

{{
    ""Test"": ""Abc""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request, false);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 411,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___requestvalidation_request_body_not_allowed()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified/requestbody/not/allowed HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {applicationJson}

{{
    ""Test"": ""Abc""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 413,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact] // This test isn't really attribute related and should be moved to new test class
        public async Task discovery_attribute___requestvalidation_missing_content_type_length()
        {
            base.SetupEnvironment();

            string toolong = new string('a', 101);
            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/requestvalidation/specified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}

{{
    ""Test"": ""{toolong}""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 415,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}