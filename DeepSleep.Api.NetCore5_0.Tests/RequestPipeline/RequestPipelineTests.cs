namespace DeepSleep.Api.NetCore.Tests.RequestPipeline
{
    using DeepSleep.Api.NetCore.Tests.Mocks;
    using FluentAssertions;
    using global::Api.DeepSleep.Controllers.RequestPipeline;
    using System.Linq;
    using System.Threading.Tasks;
    using DeepSleep.Validation;
    using Xunit;

    public class RequestPipelineTests : PipelineTestBase
    {
        [Fact]
        public async Task requestpipeline___configured_after_endpoint_item_should_be_set_but_not_available_in_endpoint()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getafterendpoint/with/static/configured/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { "CustomRequestPipelineComponent", "SET" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().BeNull();
        }

        [Fact]
        public async Task requestpipeline___configured_before_endpoint_item_should_be_set_but_not_available_in_endpoint()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getbeforeendpoint/with/static/configured/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { "CustomRequestPipelineComponent", "SET" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("SET");
        }

        [Fact]
        public async Task requestpipeline___configured_before_validation_item_should_be_set_but_not_available_in_endpoint()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getbeforevalidation/with/static/configured/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { "RequestPipelineModelValidator", "VALIDATOR-SET" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("VALIDATOR-SET");
        }

        [Fact]
        public async Task requestpipeline___attribute_after_endpoint_item_should_be_set_but_not_available_in_endpoint()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getafterendpoint/with/attribute/configured/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { "CustomRequestPipelineComponent", "SET" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().BeNull();
        }

        [Fact]
        public async Task requestpipeline___attribute_before_endpoint_item_should_be_set_but_not_available_in_endpoint()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getbeforeendpoint/with/attribute/configured/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { "CustomRequestPipelineComponent", "SET" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("SET");
        }

        [Fact]
        public async Task requestpipeline___attribute_before_validation_item_should_be_set_but_not_available_in_endpoint()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getbeforevalidation/with/attribute/configured/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { "RequestPipelineModelValidator", "VALIDATOR-SET" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("VALIDATOR-SET");
        }

        [Fact]
        public async Task requestpipeline___attribute_multiple_order_should_be_correct()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/getAttributesMultiple/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { nameof(CustomRequestPipelineComponent1), "SET-1" },
                    { nameof(CustomRequestPipelineComponent2), "SET-2" },
                    { nameof(CustomRequestPipelineComponent3), "SET-3" },
                    { nameof(CustomRequestPipelineComponent4), "SET-4" },
                    { nameof(CustomRequestPipelineComponent5), "SET-5" },
                    { nameof(CustomRequestPipelineComponent6), "SET-6" },
                    { nameof(CustomRequestPipelineComponent7), "SET-7" },
                    { nameof(CustomRequestPipelineComponent8), "SET-8" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("TEST");

            var items = apiContext.Items.Values
                .Where(v => v.ToString().StartsWith("SET-"))
                .ToList();

            items.Count.Should().Be(8);

            items[0].Should().Be("SET-7");
            items[1].Should().Be("SET-8");
            items[2].Should().Be("SET-5");
            items[3].Should().Be("SET-4");
            items[4].Should().Be("SET-3");
            items[5].Should().Be("SET-6");
            items[6].Should().Be("SET-2");
            items[7].Should().Be("SET-1");
        }

        [Fact]
        public async Task requestpipeline___mixed_multiple_order_should_be_correct()
        {
            base.SetupEnvironment();

            var request = @$"
GET https://{host}/requestpipeline/mixed/pipeline HTTP/1.1
Host: {host}
Connection: keep-alive
User-Agent: UnitTest/1.0 DEV
Accept: {applicationJson}";

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
                expectedItems: new NameValuePairs<string, string>
                {
                    { nameof(CustomRequestPipelineComponent1), "SET-1" },
                    { nameof(CustomRequestPipelineComponent2), "SET-2" },
                    { nameof(CustomRequestPipelineComponent3), "SET-3" },
                    { nameof(CustomRequestPipelineComponent4), "SET-4" },
                    { nameof(CustomRequestPipelineComponent5), "SET-5" },
                    { nameof(CustomRequestPipelineComponent6), "SET-6" },
                    { nameof(CustomRequestPipelineComponent7), "SET-7" },
                    { nameof(CustomRequestPipelineComponent8), "SET-8" },
                    { nameof(CustomRequestPipelineComponent9), "SET-9" }
                });

            var data = await base.GetResponseData<RequestPipelineModel>(response).ConfigureAwait(false);
            data.Should().NotBeNull();
            data.Value.Should().Be("TEST");

            var items = apiContext.Items.Values
                .Where(v => v.ToString().StartsWith("SET-"))
                .ToList();

            items.Count.Should().Be(9);


            /*
             * 

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent4), PipelinePlacement.BeforeEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent5), PipelinePlacement.BeforeEndpointInvocation, 0)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent3), PipelinePlacement.BeforeEndpointInvocation, 2)]

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent1), PipelinePlacement.AfterEndpointInvocation, 3)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent6), PipelinePlacement.AfterEndpointInvocation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent2), PipelinePlacement.AfterEndpointInvocation, 2)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent9), PipelinePlacement.AfterEndpointInvocation, 0)]

        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent8), PipelinePlacement.BeforeEndpointValidation, 1)]
        [ApiRequestPipeline(typeof(CustomRequestPipelineComponent7), PipelinePlacement.BeforeEndpointValidation, 0)]
             * 
             */


            items[0].Should().Be("SET-7");
            items[1].Should().Be("SET-8");

            items[2].Should().Be("SET-5");
            items[3].Should().Be("SET-4");
            items[4].Should().Be("SET-3");

            items[5].Should().Be("SET-9");
            items[6].Should().Be("SET-6");
            items[7].Should().Be("SET-2");
            items[8].Should().Be("SET-1");
        }
    }
}
