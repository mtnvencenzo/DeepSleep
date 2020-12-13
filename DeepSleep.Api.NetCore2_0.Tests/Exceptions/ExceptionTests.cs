namespace DeepSleep.Api.NetCore.Tests.Exceptions
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ExceptionTests : PipelineTestBase
    {
        // 501 Not Implemented
        // -------------------

        [Fact]
        public async Task exception_notimplemented_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/notimplemented HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 501,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_notimplemented_from_validator_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/validator/notimplemented HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 501,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Exception,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_notimplemented_from_authorization_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authorization/notimplemented HTTP/1.1
Authorization: Success test
Host: {host}
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
                expectedHttpStatus: 501,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: true,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_notimplemented_from_authentication_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authentication/notimplemented HTTP/1.1
Authorization: EX-501 test
Host: {host}
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
                expectedHttpStatus: 501,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        // 502 Bad Gateway
        // ----------------

        [Fact]
        public async Task exception_badgateway_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/badgateway HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 502,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_badgateway_from_validator_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/validator/badgateway HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 502,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Exception,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_badgateway_from_authorization_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authorization/badgateway HTTP/1.1
Authorization: Success test
Host: {host}
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
                expectedHttpStatus: 502,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_badgateway_from_authentication_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authentication/badgateway HTTP/1.1
Authorization: EX-502 test
Host: {host}
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
                expectedHttpStatus: 502,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        // 504 Gateway Timeout
        // ----------------

        [Fact]
        public async Task exception_gatewaytimeout_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/gatewaytimeout HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 504,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_gatewaytimeout_from_validator_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/validator/gatewaytimeout HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 504,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Exception,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_gatewaytimeout_from_authorization_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authorization/gatewaytimeout HTTP/1.1
Authorization: Success test
Host: {host}
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
                expectedHttpStatus: 504,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: true,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_gatewaytimeout_from_authentication_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authentication/gatewaytimeout HTTP/1.1
Authorization: EX-504 test
Host: {host}
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
                expectedHttpStatus: 504,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        // 503 Service Unavailable
        // -----------------------

        [Fact]
        public async Task exception_serviceunavailable_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/serviceunavailable HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 503,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_serviceunavailable_from_validator_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/validator/serviceunavailable HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 503,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Exception,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_serviceunavailable_from_authorization_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authorization/serviceunavailable HTTP/1.1
Authorization: Success test
Host: {host}
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
                expectedHttpStatus: 503,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: true,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_serviceunavailable_from_authentication_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authentication/serviceunavailable HTTP/1.1
Authorization: EX-503 test
Host: {host}
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
                expectedHttpStatus: 503,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        // 500 Unhandled
        // -----------------------

        [Fact]
        public async Task exception_unhandled_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/unhandled HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 500,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_unhandled_from_validator_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/validator/unhandled HTTP/1.1
Host: {host}
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
                expectedHttpStatus: 500,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.Exception,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_unhandled_from_authorization_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authorization/unhandled HTTP/1.1
Authorization: Success test
Host: {host}
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
                expectedHttpStatus: 500,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: true,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }

        [Fact]
        public async Task exception_unhandled_from_authentication_returns_correct_response()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/exceptions/authentication/unhandled HTTP/1.1
Authorization: EX-500 test
Host: {host}
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
                expectedHttpStatus: 500,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new Dictionary<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });
        }
    }
}
