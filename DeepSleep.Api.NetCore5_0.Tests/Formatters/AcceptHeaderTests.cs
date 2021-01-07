namespace DeepSleep.Api.NetCore.Tests.Formatters
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Binding;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class AcceptHeaderTests : PipelineTestBase
    {
        [Theory]
        [InlineData("application/json", "application/json")]
        [InlineData("text/json", "text/json")]
        [InlineData("application/json; q=0, text/json; q=1", "text/json")]
        [InlineData("application/json; q=0.2, application/xml; q=0.1", "application/json")]
        [InlineData("application/json; q=0.1, application/xml; q=0.2, text/json; q=0.3", "text/json")]
        [InlineData("application/*; q=0.2, application/xml; q=0.1", "application/json")]
        public async Task accept___json_successful(string accept, string exectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: exectedContentType,
                shouldHaveResponse: true,
                expectedContentLength: 358,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().BeTrue();
        }

        [Theory]
        [InlineData("application/json", "application/json")]
        [InlineData("text/json", "text/json")]
        [InlineData("application/json; q=0, text/json; q=1", "text/json")]
        [InlineData("application/json; q=0.2, application/xml; q=0.1", "application/json")]
        [InlineData("application/json; q=0.1, application/xml; q=0.2, text/json; q=0.3", "text/json")]
        [InlineData("application/*; q=0.2, application/xml; q=0.1", "application/json")]
        public async Task accept___json_successful_for_enabled_head(string accept, string exectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: exectedContentType,
                shouldHaveResponse: true,
                expectedContentLength: 358,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("application/xml", "application/xml")]
        [InlineData("text/xml", "text/xml")]
        [InlineData("application/xml; q=0, text/xml; q=1", "text/xml")]
        [InlineData("application/xml; q=0.2, application/json; q=0.1", "application/xml")]
        [InlineData("application/xml; q=0.1, application/json; q=0.2, text/xml; q=0.3", "text/xml")]
        public async Task accept___xml_successful(string accept, string exectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: exectedContentType,
                shouldHaveResponse: true,
                expectedContentLength: 1188,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().BeTrue();
        }

        [Fact]
        public async Task accept___xml_xaccexpt_header_successful()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept?xcorrelationid={correlationId} HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
X-Accept: {applicationXml}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationXml,
                shouldHaveResponse: true,
                expectedContentLength: 1188,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().BeTrue();
        }

        [Fact]
        public async Task accept___xml_xaccexpt_query_successful()
        {
            base.SetupEnvironment(services =>
            {
            });

            var now = DateTime.UtcNow.ToString("r");
            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept?xcorrelationid={correlationId}&xaccept={applicationXml} HTTP/1.1
X-Date: {now}
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationXml,
                shouldHaveResponse: true,
                expectedContentLength: 1188,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().BeTrue();
        }

        [Theory]
        [InlineData("application/xml", "application/xml")]
        [InlineData("text/xml", "text/xml")]
        [InlineData("application/xml; q=0, text/xml; q=1", "text/xml")]
        [InlineData("application/xml; q=0.2, application/json; q=0.1", "application/xml")]
        [InlineData("application/xml; q=0.1, application/json; q=0.2, text/xml; q=0.3", "text/xml")]
        public async Task accept___xml_successful_for_enabled_head(string accept, string exectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: exectedContentType,
                shouldHaveResponse: true,
                expectedContentLength: 1188,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("application/*", "application/json")]
        [InlineData("text/*", "text/json")]
        [InlineData("*/*", "application/json")]
        [InlineData("", "application/json")]
        public async Task accept___defaults_json_successful(string accept, string exectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: exectedContentType,
                shouldHaveResponse: true,
                expectedContentLength: 358,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().BeTrue();
        }

        [Theory]
        [InlineData("application/*", "application/json")]
        [InlineData("text/*", "text/json")]
        [InlineData("*/*", "application/json")]
        [InlineData("", "application/json")]
        public async Task accept___defaults_json_successful_for_enabled_head(string accept, string exectedContentType)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: exectedContentType,
                shouldHaveResponse: true,
                expectedContentLength: 358,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Fact]
        public async Task accept___defaults_json_no_accept_header()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedContentLength: 358,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();

            data.BoolVar.Should().BeTrue();
        }

        [Fact]
        public async Task accept___defaults_json_no_accept_header_for_enabled_head()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedContentLength: 358,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<SimpleUrlBindingRs>(response).ConfigureAwait(false);
            data.Should().BeNull();
        }

        [Theory]
        [InlineData("text/format")]
        [InlineData("application/*; q=0")]
        [InlineData("application/*; q=0, text/format, text/json; q=0")]
        [InlineData("application/json-patch+json")]
        [InlineData("multipart/form-data")]
        [InlineData("application/x-www-form-urlencoded")]
        public async Task accept___not_acceptable(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-Allow-Accept", $"application/json, text/json, text/xml, application/xml"}
                });
        }

        [Theory]
        [InlineData("text/format")]
        [InlineData("application/*; q=0")]
        [InlineData("application/*; q=0, text/format, text/json; q=0")]
        [InlineData("application/json-patch+json")]
        [InlineData("multipart/form-data")]
        [InlineData("application/x-www-form-urlencoded")]
        public async Task accept___not_acceptable_for_enabled_head(string accept)
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
HEAD https://{host}/formatters/accept HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 406,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-Allow-Accept", $"application/json, text/json, text/xml, application/xml"}
                });
        }
    }
}
