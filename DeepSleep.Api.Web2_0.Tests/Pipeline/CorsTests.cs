namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System.Threading.Tasks;
    using Xunit;

    public class CorsTests : PipelineTestBase
    {
        [Fact]
        public async Task cors_preflight___access_get_for_get_with_disabled_head()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Headers", "content-type"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_disabled_head()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD
Access-Control-Request-Headers: Content-Type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Headers", "Content-Type"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_enabled_head()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Headers", "content-type"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_enabled_head()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Headers", "content-type"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // Max Age Tests
        // --------------------

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_disabled_head_max_age()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled/maxage HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Headers", "content-type"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "100"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_disabled_head_mag_age()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled/maxage HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD
Access-Control-Request-Headers: Content-Type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Headers", "Content-Type"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_enabled_head_max_age()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled/maxage HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Headers", "content-type"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "150"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_enabled_head_max_age()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled/maxage HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Headers", "content-type"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "150"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // Allow Header Tests
        // --------------------

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_disabled_head_allow_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled/allowheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Headers", "Content-Type, X-CorrelationId" },
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_disabled_head_allow_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled/allowheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD
Access-Control-Request-Headers: Content-Type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Headers", "Content-Type"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_enabled_head_allow_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled/allowheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Headers", "Content-Type, X-CorrelationId"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_enabled_head_allow_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled/allowheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD
Access-Control-Request-Headers: content-type";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Headers", "Content-Type, X-CorrelationId"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_disabled_head_no_access_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_disabled_head_no_access_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET"},
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_get_for_get_with_enabled_head_no_access_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: GET";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task cors_preflight___access_head_for_get_with_enabled_head_no_access_headers()
        {
            base.SetupEnvironment();

            var request = @$"
OPTIONS https://{host}/head/configured/enabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: http://some-origin.com
Access-Control-Request-Method: HEAD";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 204,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Methods", "GET, HEAD"},
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", "http://some-origin.com"},
                    { "Access-Control-Max-Age", "600"},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // Origin Header Tests
        // --------------------

        [Theory]
        [InlineData("http://some-origin.com", "http://some-origin.com")]
        [InlineData("http://test-origin.com", "http://test-origin.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___get_head_disabled_specified_request_origin_configured_all(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeFalse();
        }

        [Theory]
        [InlineData("http://some-origin.com")]
        [InlineData("http://test-origin.com")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task cors_origin___head_head_disabled_specified_request_origin_configured_all(string origin)
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow","GET" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com", "http://some-origin.com")]
        [InlineData("http://test-origin.com", "http://test-origin.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___get_head_enabled_specified_request_origin_configured_all(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/head/configured/enabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 19,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeTrue();
        }

        [Theory]
        [InlineData("http://some-origin.com", "http://some-origin.com")]
        [InlineData("http://test-origin.com", "http://test-origin.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___head_head_enabled_specified_request_origin_configured_all(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/head/configured/enabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 19,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com")]
        [InlineData("http://test-origin.com")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task cors_origin___get_specified_request_origin_configured_all_but_405_method_not_found_endpoint(string origin)
        {
            base.SetupEnvironment();

            var request = @$"
POST https://{host}/head/configured/disabled HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow","GET" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com")]
        [InlineData("http://test-origin.com")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task cors_origin___get_specified_request_origin_configured_all_but_404_not_found_endpoint(string origin)
        {
            base.SetupEnvironment();

            var request = @$"
POST https://{host}/head/configured/disabled/not/real HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 404,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com", "")]
        [InlineData("http://test-origin.com", "")]
        [InlineData("https://test1.com", "https://test1.com")]
        [InlineData("https://test2.com", "https://test2.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___get_head_disabled_specified_request_origin_configured_specific(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/head/configured/disabled/origin HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeFalse();
        }

        [Theory]
        [InlineData("http://some-origin.com")]
        [InlineData("http://test-origin.com")]
        [InlineData("https://test1.com")]
        [InlineData("https://test2.com")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task cors_origin___head_head_disabled_specified_request_origin_configured_specific(string origin)
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/head/configured/disabled/origin HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow","GET" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com", "")]
        [InlineData("http://test-origin.com", "")]
        [InlineData("https://test1.com", "https://test1.com")]
        [InlineData("https://test2.com", "https://test2.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___get_head_enabled_specified_request_origin_configured_specific(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/head/configured/enabled/origin HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 19,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeTrue();
        }

        [Theory]
        [InlineData("http://some-origin.com", "")]
        [InlineData("http://test-origin.com", "")]
        [InlineData("https://test1.com", "https://test1.com")]
        [InlineData("https://test2.com", "https://test2.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___head_head_enabled_specified_request_origin_configured_specific(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/head/configured/enabled/origin HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 19,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com", "http://some-origin.com")]
        [InlineData("http://test-origin.com", "http://test-origin.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___get_head_disabled_specified_request_exposeheaders_configured(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/head/configured/disabled/exposeheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

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
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "true"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" },
                    { "Access-Control-Expose-Headers", "X-Header1, X-Header2" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeFalse();
        }

        [Theory]
        [InlineData("http://some-origin.com")]
        [InlineData("http://test-origin.com")]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public async Task cors_origin___head_head_disabled_specified_request_exposeheaders_configured(string origin)
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/head/configured/disabled/exposeheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 405,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Allow","GET" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("http://some-origin.com", "")]
        [InlineData("http://test-origin.com", "")]
        [InlineData("https://test1.com", "https://test1.com")]
        [InlineData("https://test2.com", "https://test2.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___get_head_enabled_specified_request_exposeheaders_configured(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/head/configured/enabled/exposeheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 19,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" },
                    { "Access-Control-Expose-Headers", "X-Header1, X-Header2" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Configured.Should().BeTrue();
        }

        [Theory]
        [InlineData("http://some-origin.com", "")]
        [InlineData("http://test-origin.com", "")]
        [InlineData("https://test1.com", "https://test1.com")]
        [InlineData("https://test2.com", "https://test2.com")]
        [InlineData(" ", "")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public async Task cors_origin___head_head_enabled_specified_request_origin_exposeheaders_configured(string origin, string expectedOrigin)
        {
            base.SetupEnvironment();

            var request = @$"
HEAD https://{host}/head/configured/enabled/exposeheaders HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip, deflate
Origin: {origin}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                shouldHaveResponse: true,
                expectedContentLength: 19,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "Access-Control-Allow-Credentials", "false"},
                    { "Access-Control-Allow-Origin", expectedOrigin},
                    { "Vary", "Origin" },
                    { "Access-Control-Expose-Headers", "X-Header1, X-Header2" }
                });

            var data = await base.GetResponseData<RequestHeadModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
