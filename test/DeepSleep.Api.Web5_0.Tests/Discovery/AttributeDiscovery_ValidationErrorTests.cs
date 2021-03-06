﻿namespace DeepSleep.Api.Web.Tests.Discovery
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Auth;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Models;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AttributeDiscovery_ValidationErrorTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingerror_defaults_correctly()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/default?CustomInt=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("'CustomInt' is in an incorrect format and could not be bound.");
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingvalueerror_defaults_correctly()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/default?queryValue=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("Uri type conversion for 'queryValue' with value 'abc' could not be converted to type Nullable`1.");
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingerror_specified_empty()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/specified/empty?CustomInt=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingvalueerror_specified_empty()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/specified/empty?queryValue=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: false,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingerror_specified_no_replacements()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/specified/custom/no/replacements?CustomInt=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("uriBindingError-test");
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingvalueerror_specified_no_replacements()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/specified/custom/no/replacements?queryValue=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("uriBindingValueError-test");
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingerror_specified_with_replacements()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/specified/custom/with/replacements?CustomInt=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("CustomInt");
        }

        [Fact]
        public async Task discovery_attribute___validationerror_urlbindingvalueerror_specified_with_replacements()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/validationerror/specified/custom/with/replacements?queryValue=abc HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("queryValue-abc-Nullable`1");
        }

        [Fact]
        public async Task discovery_attribute___deserializationerror_400_statuscode_for_strict()
        {
            base.SetupEnvironment();


            var request = @$"
POST https://{host}/discovery/attribute/validationerror/400/deserialization/error/for/strict HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {applicationJson}

<AttributeDiscoveryModel>
</AttributeDiscoveryModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 400,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("The request body could not be deserialized.");
        }

        [Fact]
        public async Task discovery_attribute___deserializationerror_400_statuscode_for_common()
        {
            base.SetupEnvironment();


            var request = @$"
POST https://{host}/discovery/attribute/validationerror/400/deserialization/error/for/common HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}
Content-Type: {applicationJson}

<AttributeDiscoveryModel>
</AttributeDiscoveryModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 400,
                shouldHaveResponse: true,
                expectedAuthenticatedBy: AuthenticationType.Provider,
                expectedAuthorizedBy: AuthorizationType.Provider,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("The request body could not be deserialized.");
        }
    }
}
