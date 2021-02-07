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

    public class AttributeDiscovery_ReadWriteTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___readwrite_defaults_correctly()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/readwrite/default HTTP/1.1
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
        public async Task discovery_attribute___readwrite_acceptheader_override()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/readwrite/acceptheader/override HTTP/1.1
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
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Theory]
        [InlineData("text/json")]
        [InlineData("text/xml")]
        [InlineData("text/application")]
        public async Task discovery_attribute___readwrite_supported_writeablemediatypes__nomatch(string accept)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/readwrite/writeablemediatypes/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {accept}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-Allow-Accept", "application/xml" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___readwrite_supported_writeablemediatypes__match()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/readwrite/writeablemediatypes/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationXml}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationXml,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }

        [Theory]
        [InlineData("text/json")]
        [InlineData("text/xml")]
        [InlineData("application/xml")]
        [InlineData("text/application")]
        [InlineData("application/plain")]
        [InlineData("application/json")]
        public async Task discovery_attribute___readwrite_custom_writerresolver_nomatch(string accept)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/readwrite/writerresolver/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {accept}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.None,
                expectedAuthorizedBy: AuthorizationType.None,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-Allow-Accept", "text/plain" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___readwrite_custom_writerresolver_match()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute/readwrite/writerresolver/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {textPlain}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: textPlain,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<string>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Should().Be("Posted");
        }

        [Fact]
        public async Task discovery_attribute___readwrite_supported_readablemediatypes__nomatch()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/readwrite/readablemediatypes/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {applicationJson}

{{
    ""PostValue"": ""Posted""
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
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-Allow-Content-Types", "application/xml" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___readwrite_supported_readablemediatypes__match()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/readwrite/readablemediatypes/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {applicationXml}

<AttributeDiscoveryModel>
    <PostValue>Posted</PostValue>
</AttributeDiscoveryModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
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
            data.PostValue.Should().Be("Posted");
        }

        [Fact]
        public async Task discovery_attribute___readwrite_custom_readerresolver_nomatch()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/readwrite/readerresolver/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {textJson}

{{
    ""PostValue"": ""Posted""
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
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-Allow-Content-Types", "text/plain" }
                });

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___readwrite_custom_readerresolver_match()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/discovery/attribute/readwrite/readerresolver/override HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {textPlain}

Posted";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
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
            data.PostValue.Should().Be("Posted");
        }
    }
}