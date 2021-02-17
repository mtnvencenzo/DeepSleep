namespace DeepSleep.Api.Web.Tests.Pipeline
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Pipeline;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ReadWriteConfiguration_WritableTypesTests : PipelineTestBase
    {
        [Theory]
        [InlineData("text/json")]
        [InlineData("application/json")]
        [InlineData("application/json; q=1, text/json; q=0.9, application/xml; q=0, text/xml; q=0")]
        [InlineData("application/json; q=1, text/json; q=0.9, application/xml; q=0, text/xml; q=0, other/xml; q=0")]
        [InlineData("other/xml; q=0")]
        public async Task writeabletypes___should_only_allow_xml(string accept)
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeabletypes/text-xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}";

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
                    { "X-Allow-Accept", "text/xml" }
                });
        }

        [Theory]
        [InlineData("TEXT/xml")]
        [InlineData("application/json; q=1, other/xml; q=0.1, text/XML")]
        [InlineData("application/json; q=1, other/xml; q=0, other/*; q=0.2, TEXT/XML")]
        [InlineData("other/*, text/XML")]
        [InlineData("text/*")]
        [InlineData("application/*, text/xml; q=0.9")]
        public async Task writeabletypes___uses_writeable_types_xml(string accept)
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeabletypes/text-xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}
";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: 200,
                expectedContentType: textXml,
                shouldHaveResponse: true,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    
                });

            var data = await base.GetResponseData<ReadWriteOverrideModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.AcceptHeader.Should().Be(accept);
            data.AcceptHeaderOverride.Should().BeNull();
            data.ReadableTypes.Should().BeNull();
            data.AcceptHeaderOverride.Should().Be(null);
            data.WriteableTypes.Should().Be("text/xml,other/xml");
        }

        [Theory]
        [InlineData("other/xml")]
        [InlineData("other/*")]
        [InlineData("application/*; q=1, other/xml; q=0.1, text/xml; q=0")]
        [InlineData("text/json, application/*; q=1, other/xml; q=0.1, text/xml; q=0")]
        public async Task writeabletypes___uses_writeable_types_but_none_match_existing_formatters(string accept)
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/pipeline/readwrite/configuration/writeabletypes/text-xml HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {accept}";

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
                    { "X-Allow-Accept", $"text/xml" }
                });
        }
    }
}


