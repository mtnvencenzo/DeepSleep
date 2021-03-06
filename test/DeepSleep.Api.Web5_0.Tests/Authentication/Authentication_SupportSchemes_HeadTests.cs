﻿namespace DeepSleep.Api.Web.Tests.Authentication
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Authentication;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class Authentication_SupportSchemes_HeadTests : PipelineTestBase
    {
        // Single Providers
        // ------------------

        [Fact]
        public async Task authentication_head___single_supported_scheme_unauthenticated_no_header()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/single/supported/schemes HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                });
        }

        [Fact]
        public async Task authentication_head___single_supported_scheme_unauthenticated_header_no_scheme()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/single/supported/schemes HTTP/1.1
Host: {host}
Authorization:
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                });
        }

        [Fact]
        public async Task authentication_head___single_supported_scheme_unauthenticated_header_scheme_no_value()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/single/supported/schemes HTTP/1.1
Host: {host}
Authorization: Token
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: "Token",
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                });
        }

        [Fact]
        public async Task authentication_head___single_supported_scheme_unauthenticated_header_scheme_invalid_value()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/single/supported/schemes HTTP/1.1
Host: {host}
Authorization: Token asdasdd
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: "Token",
                expectedAuthenticationValue: "asdasdd",
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                });
        }

        [Fact]
        public async Task authentication_head___single_supported_scheme_unauthenticated_header_scheme_valid_value()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/single/supported/schemes HTTP/1.1
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
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: "Token",
                expectedAuthenticationValue: staticToken,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // Multiple Providers
        // ------------------

        [Fact]
        public async Task authentication_head___multiple_supported_scheme_unauthenticated_no_header()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/multiple/supported/schemes HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" }
                });
        }

        [Fact]
        public async Task authentication_head___multiple_supported_scheme_unauthenticated_header_no_scheme()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/multiple/supported/schemes HTTP/1.1
Host: {host}
Authorization:
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___multiple_scheme_unauthenticated_header_scheme_no_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/multiple/supported/schemes HTTP/1.1
Host: {host}
Authorization: {scheme}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___multiple_scheme_unauthenticated_header_scheme_invalid_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/multiple/supported/schemes HTTP/1.1
Host: {host}
Authorization: {scheme} asdlkasdk
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: "asdlkasdk",
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___multiple_supported_scheme_unauthenticated_header_scheme_valid_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/multiple/supported/schemes HTTP/1.1
Host: {host}
Authorization: {scheme} {staticToken}
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
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: staticToken,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // Supported Providers Not Defined
        // -------------------------------

        [Fact]
        public async Task authentication_head___notdefined_supported_scheme_unauthenticated_no_header()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/not/defined/supported/schemes HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Fact]
        public async Task authentication_head___notdefined_supported_scheme_unauthenticated_header_no_scheme()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/not/defined/supported/schemes HTTP/1.1
Host: {host}
Authorization:
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___notdefined_supported_scheme_unauthenticated_header_scheme_no_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/not/defined/supported/schemes HTTP/1.1
Host: {host}
Authorization: {scheme}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___notdefined_supported_scheme_unauthenticated_header_scheme_invalid_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/not/defined/supported/schemes HTTP/1.1
Host: {host}
Authorization: {scheme} ksjdfkjshdfjs
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: "ksjdfkjshdfjs",
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___notdefined_supported_scheme_unauthenticated_header_scheme_valid_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/not/defined/supported/schemes HTTP/1.1
Host: {host}
Authorization: {scheme} {staticToken}
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
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: staticToken,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        // Supported Providers Empty
        // -------------------------------

        [Fact]
        public async Task authentication_head___empty_supported_scheme_unauthenticated_no_header()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/empty/defined/supported/scheme HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Fact]
        public async Task authentication_head___empty_supported_scheme_unauthenticated_header_no_scheme()
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/empty/defined/supported/scheme HTTP/1.1
Host: {host}
Authorization:
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.None,
                expectedAuthenticationScheme: null,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___empty_supported_scheme_unauthenticated_header_scheme_no_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/empty/defined/supported/scheme HTTP/1.1
Host: {host}
Authorization: {scheme}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: null,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___empty_supported_scheme_unauthenticated_header_scheme_invalid_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/empty/defined/supported/scheme HTTP/1.1
Host: {host}
Authorization: {scheme} ksjdfkjshdfjs
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 401,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                expectedAuthenticationResult: false,
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: "ksjdfkjshdfjs",
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "WWW-Authenticate", $"Token realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Token2 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-500 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-501 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-502 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-503 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"EX-504 realm=\"Api-Unit-Test\"" },
                    { "WWW-Authenticate", $"Success realm=\"Unit-Test\"" }
                });
        }

        [Theory]
        [InlineData("Token")]
        [InlineData("Token2")]
        public async Task authentication_head___empty_supported_scheme_unauthenticated_header_scheme_valid_value(string scheme)
        {
            base.SetupEnvironment();


            var request = @$"
HEAD https://{host}/authentication/empty/defined/supported/scheme HTTP/1.1
Host: {host}
Authorization: {scheme} {staticToken}
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
                expectedAuthenticatedBy: Auth.AuthenticationType.Provider,
                expectedAuthenticationScheme: scheme,
                expectedAuthenticationValue: staticToken,
                expectedContentLength: 16,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<AuthenticatedModel>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }
    }
}
