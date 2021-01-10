namespace DeepSleep.Api.NetCore.Tests.Ping
{
    using DeepSleep.Api.NetCore.TestBase;
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using DeepSleep.Validation;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class OpenApiTests : PipelineTestBase
    {
        [Fact]
        public async Task openapiv3___validate_success()
        {
            base.SetupEnvironment(services =>
            {
            });

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
            data.Should().NotBeNull();
            data.Should().NotBeEmpty();
            data.Should().StartWith("{");
            data.Should().EndWith("}");

            var rawSchema = Encoding.UTF8.GetString(TestResources.openapi_v3);
            var resolver = new JSchemaUrlResolver();

            var schema = JSchema.Parse(rawSchema, resolver);
            var json = JObject.Parse(data);
 
            bool valid = json.IsValid(schema);
            valid.Should().BeTrue();
        }
    }
}
