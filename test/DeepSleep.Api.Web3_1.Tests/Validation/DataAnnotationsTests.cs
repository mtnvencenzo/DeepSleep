namespace DeepSleep.Api.Web.Tests.Validation
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Models;
    using System.Threading.Tasks;
    using Xunit;

    public class DataAnnotationsTests : PipelineTestBase
    {
        [Fact]
        public async Task dataannotations___correctly_validates_invalid_uri_model()
        {
            base.SetupEnvironment();

            var request = @$"
POST https://{host}/validators/dataannotations/uri/and/body/2020 HTTP/1.1
Accept: {applicationJson}
Content-Type: {applicationJson}

{{
    ""Value"": ""Test""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("Uri value is not in specified format");
        }

        [Fact]
        public async Task dataannotations___correctly_validates_invalid_uri_model_with_invalid_body()
        {
            base.SetupEnvironment();

            var request = @$"
POST https://{host}/validators/dataannotations/uri/and/body/2020 HTTP/1.1
Accept: {applicationJson}
Content-Type: {applicationJson}

{{
    ""Value"": ""2020""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.uriBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("Uri value is not in specified format");
        }

        [Fact]
        public async Task dataannotations___correctly_validates_invalid_body_model()
        {
            base.SetupEnvironment();

            var request = @$"
POST https://{host}/validators/dataannotations/uri/and/body/TEST HTTP/1.1
Accept: {applicationJson}
Content-Type: {applicationJson}

{{
    ""Value"": ""2020""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.bodyBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("Body value is not in specified format");
        }

        [Fact]
        public async Task dataannotations___correctly_validates_endpoint_when_uri_and_body_valid()
        {
            base.SetupEnvironment();

            var request = @$"
POST https://{host}/validators/dataannotations/uri/and/body/TEST HTTP/1.1
Accept: {applicationJson}
Content-Type: {applicationJson}

{{
    ""Value"": ""TESTER""
}}";

            using var httpContext = new MockHttpContext(this.ServiceProvider, request);
            var apiContext = await Invoke(httpContext).ConfigureAwait(false);
            var response = httpContext.Response;

            base.AssertResponse(
                apiContext: apiContext,
                response: response,
                expectedHttpStatus: base.bodyBindingErrorStatusCode,
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Failed,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CommonErrorResponse>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Messages.Should().HaveCount(1);
            data.Messages[0].ErrorMessageStr.Should().Be("Body value and Uri value must be the same");
        }
    }
}
