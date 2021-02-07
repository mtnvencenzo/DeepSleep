namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ReadWriteConfiguration_ReadableTypesTests : PipelineTestBase
    {
        [Theory]
        [InlineData("application/json")]
        [InlineData("other/xml")]
        [InlineData("application/xml")]
        [InlineData("text/json")]
        [InlineData("image/jog")]
        public async Task readabletypes___uses_readable_types_but_not_existent_xml(string contentType)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readabletypes/text-xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: application/json
Content-Type: {contentType}
X-CorrelationId: {correlationId}

<ReadWriteOverrideModel>
    <AcceptHeader>test</AcceptHeader>
</ReadWriteOverrideModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 415,
                shouldHaveResponse: false,
                expectedValidationState: ApiValidationState.NotAttempted,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"},
                    { "X-Allow-Content-Types", "text/xml, other/xml" }
                });
        }

        [Theory]
        [InlineData("text/xml")]
        public async Task readabletypes___uses_readable_types_xml(string contentType)
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
POST https://{host}/pipeline/readwrite/configuration/readabletypes/text-xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {textJson}
Content-Type: {contentType}
X-CorrelationId: {correlationId}

<ReadWriteOverrideModel>
    <AcceptHeader>test</AcceptHeader>
</ReadWriteOverrideModel>";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textJson,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(textJson);
            data.AcceptHeaderOverride.Should().BeNull();
            data.ReadableTypes.Should().Be("text/xml,other/xml");
            data.AcceptHeaderOverride.Should().Be(null);
            data.WriteableTypes.Should().BeNull();
        }
    }
}


