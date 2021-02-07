namespace DeepSleep.Api.Web.Tests.OpenApi
{
    using DeepSleep.Api.Web.TestBase;
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiTests : PipelineTestBase
    {
        [Fact]
        public async Task openapiv3___default_version_json_validate_success()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/openapi/v3/doc HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().StartWith("{");
            data.Should().EndWith("}");

            var rawSchema = Encoding.UTF8.GetString(TestResources.openapi_v3);
            var resolver = new JSchemaUrlResolver();

            var schema = JSchema.Parse(rawSchema, resolver);
            var json = JObject.Parse(data);

            IList<ValidationError> errors = null;
            bool valid = json.IsValid(schema, out errors);
            valid.Should().BeTrue();
        }

        [Fact]
        public async Task openapiv3___specific_version_json_validate_success()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/openapi/v3/doc?version=3 HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().StartWith("{");
            data.Should().EndWith("}");

            var rawSchema = Encoding.UTF8.GetString(TestResources.openapi_v3);
            var resolver = new JSchemaUrlResolver();

            var schema = JSchema.Parse(rawSchema, resolver);
            var json = JObject.Parse(data);

            IList<ValidationError> errors = null;
            bool valid = json.IsValid(schema, out errors);
            valid.Should().BeTrue();
        }

        [Fact]
        public async Task openapiv3___yaml_validate_success()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/openapi/v3/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationYaml}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationYaml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().NotStartWith("{");
            data.Should().NotEndWith("}");
        }

        [Fact]
        public async Task openapiv2___default_version_json_validate_success()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/openapi/v2/doc?version=2 HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().StartWith("{");
            data.Should().EndWith("}");

            var rawSchema = Encoding.UTF8.GetString(TestResources.openapi_v2);
            var resolver = new JSchemaUrlResolver();

            var schema = JSchema.Parse(rawSchema, resolver);
            var json = JObject.Parse(data);

            IList<ValidationError> errors = null;
            bool valid = json.IsValid(schema, out errors);
            valid.Should().BeTrue();
        }

        [Fact]
        public async Task openapiv2___specific_version_json_validate_success()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/openapi/v2/doc?version=2 HTTP/1.1
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
                expectedHttpStatus: 200,
                expectedContentType: applicationJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().StartWith("{");
            data.Should().EndWith("}");

            var rawSchema = Encoding.UTF8.GetString(TestResources.openapi_v2);
            var resolver = new JSchemaUrlResolver();

            var schema = JSchema.Parse(rawSchema, resolver);
            var json = JObject.Parse(data);

            IList<ValidationError> errors = null;
            bool valid = json.IsValid(schema, out errors);
            valid.Should().BeTrue();
        }

        [Fact]
        public async Task openapiv2___yaml_validate_success()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/openapi/v2/doc?version=2 HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationYaml}
X-CorrelationId: {correlationId}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: applicationYaml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().NotStartWith("{");
            data.Should().NotEndWith("}");
        }
    }
}
