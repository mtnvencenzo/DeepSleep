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

    public class AttributeDiscovery_AuthenticationTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___authentication_anonymous_true()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/allowanonymous/true HTTP/1.1
Date: {utcNow.ToString("r")}
Host: {host}
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
                expectedAuthenticatedBy: AuthenticationType.Anonymous,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___authentication_anonymous_false_no_auth_header()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/allowanonymous/false HTTP/1.1
Date: {utcNow.ToString("s")}
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Success realm=\"Unit-Test\"" },
                    { "WWW-Authenticate", "EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-504 realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_anonymous_false_invalid_auth_header()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/allowanonymous/false?xdate={utcNow.ToUnixTimeSeconds()} HTTP/1.1
Authorization: Token {staticToken}1
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Success realm=\"Unit-Test\"" },
                    { "WWW-Authenticate", "EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-504 realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_anonymous_false_valid_auth_header()
        {
            base.SetupEnvironment();

            var utcNow = DateTimeOffset.UtcNow;
            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/allowanonymous/false HTTP/1.1
Date: {utcNow.ToString("r").Replace("GMT", "UTC")}
Authorization: Token {staticToken}
Host: {host}
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
                expectedAuthenticatedBy: AuthenticationType.Provider,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_null_no_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/notspecified HTTP/1.1
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Success realm=\"Unit-Test\"" },
                    { "WWW-Authenticate", "EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-504 realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_null_valid_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/notspecified HTTP/1.1
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
                 expectedContentType: applicationJson,
                 expectedValidationState: ApiValidationState.Succeeded,
                 expectedAuthenticatedBy: AuthenticationType.Provider,
                 extendedHeaders: new NameValuePairs<string, string>
                 {
                 });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_null_invalid_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/notspecified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}1
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Success realm=\"Unit-Test\"" },
                    { "WWW-Authenticate", "EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-504 realm=\"Api-Unit-Test\"" }
                });
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_empty_no_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified/empty HTTP/1.1
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Success realm=\"Unit-Test\"" },
                    { "WWW-Authenticate", "EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-504 realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_empty_invalid_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified/empty HTTP/1.1
Host: {host}
Authorization: Token {staticToken}1
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "Success realm=\"Unit-Test\"" },
                    { "WWW-Authenticate", "EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", "EX-504 realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_empty_valid_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified/empty HTTP/1.1
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
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_specified_no_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified HTTP/1.1
Host: {host}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_specified_valid_unsupported_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified HTTP/1.1
Host: {host}
Authorization: Token2 {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_specified_invalid_supported_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified HTTP/1.1
Host: {host}
Authorization: Token {staticToken}1
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticatedBy: AuthenticationType.None,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", "Token realm=\"Api-Unit-Test\"" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___authentication_schemes_specified_valid_supported_auth_header()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/authentication/schemes/specified HTTP/1.1
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
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }
    }
}
