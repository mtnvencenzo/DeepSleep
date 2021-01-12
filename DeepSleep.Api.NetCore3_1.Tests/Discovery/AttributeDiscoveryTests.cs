﻿namespace DeepSleep.Api.NetCore.Tests.Discovery
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.Discovery;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using DeepSleep.Validation;

    public class AttributeDiscoveryTests : PipelineTestBase
    {
        [Fact]
        public async Task discovery_attribute___has_route_registered()
        {
            base.SetupEnvironment(services =>
            {
            });

            var correlationId = Guid.NewGuid();
            var request = @$"
GET https://{host}/discovery/attribute HTTP/1.1
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

            var data = await base.GetResponseData<AttributeDiscoveryModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("Test");
        }
    }
}