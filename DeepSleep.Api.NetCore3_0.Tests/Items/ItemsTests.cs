namespace DeepSleep.Api.NetCore.Tests.Items
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Items;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ItemsTests : PipelineTestBase
    {
        [Fact]
        public async Task items___adds_upserts_and_retreives_item()
        {
            base.SetupEnvironment();

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/context/items HTTP/1.1
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
                shouldHaveResponse: true,
                expectedContentType: applicationJson,
                expectedValidationState: ApiValidationState.Succeeded,
                extendedHeaders: new NameValuePairs<string, string>
                {
                    { "X-CorrelationId", $"{correlationId}"}
                });

            var data = await base.GetResponseData<ItemsRs>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Found1.Should().BeTrue();
            data.Value1.Should().Be("TestItemValue");
            data.Found2.Should().BeTrue();
            data.Value2.Should().Be("TestItemValue2");
            data.Found3.Should().BeFalse();
            data.Value3.Should().BeNull();
        }
    }
}
