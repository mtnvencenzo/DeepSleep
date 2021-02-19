namespace DeepSleep.Api.OpenApiCheckTests
{
    using DeepSleep.Api.OpenApiCheckTests._resources;
    using DeepSleep.Api.OpenApiCheckTests.Mocks;
    using FluentAssertions;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiTests : TestBase
    {
        [Fact]
        public async Task openapiv3___json_validate_success()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/openapi/v3/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
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
        public async Task openapiv3___json_no_accept_validate_success()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/openapi/v3/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
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


            var request = @$"
GET https://{host}/openapi/v3/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationYaml}";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
            data.Should().NotBeEmpty();
            data.Should().NotStartWith("{");
            data.Should().NotEndWith("}");
        }

        [Fact]
        public async Task openapiv3___yaml_format_specified_validate_success()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/openapi/v3/doc?format=yaml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
            data.Should().NotBeEmpty();
            data.Should().NotStartWith("{");
            data.Should().NotEndWith("}");
        }

        [Fact]
        public async Task openapiv2___json_validate_success()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/openapi/v2/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
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
        public async Task openapiv2___json_no_accept_validate_success()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/openapi/v2/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
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


            var request = @$"
GET https://{host}/openapi/v2/doc HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationYaml}";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            //System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
            data.Should().NotBeEmpty();
            data.Should().NotStartWith("{");
            data.Should().NotEndWith("}");
        }

        [Fact]
        public async Task openapiv2___yaml_format_specified_validate_success()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/openapi/v2/doc?format=yaml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

            using var httpContext = new MockHttpContext(base.serviceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            response.Should().NotBeNull();

            var data = await base.GetResponseDataString(response).ConfigureAwait(false);

            // System.Diagnostics.Debug.Write(data);

            data.Should().NotBeNullOrWhiteSpace();
            data.Should().NotBeEmpty();
            data.Should().NotStartWith("{");
            data.Should().NotEndWith("}");
        }
    }
}
