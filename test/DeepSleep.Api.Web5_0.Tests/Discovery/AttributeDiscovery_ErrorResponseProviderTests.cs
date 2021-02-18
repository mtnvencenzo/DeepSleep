namespace DeepSleep.Api.Web.Tests.Discovery
{
    using DeepSleep.Api.Web.Tests.Mocks;
    using DeepSleep.Auth;
    using DeepSleep.Validation;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Discovery;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AttributeDiscovery_ErrorResponseProviderTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___errorresponseprovider_defaults_correctly()
        {
            base.SetupEnvironment();


            var request = @$"
GET https://{host}/discovery/attribute/errorresponseprovider/default HTTP/1.1
Host: {host}
Authorization: Token {staticToken}
Accept: {applicationJson}";

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
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                });

            var data = await base.GetResponseData<CustomResponseErrorObject>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Test.Should().Be("Value");
            data.Errors.Should().NotBeNull();
            data.Errors.Should().HaveCount(2);
            data.Errors[0].Should().Be("test-error-1");
            data.Errors[1].Should().Be("test-error-2");

        }
    }
}