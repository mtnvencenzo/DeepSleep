namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestNotFoundTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseForNullRouteInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = null
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsFalseForNullTemplateInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = null
                }
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsFalseForNullEndpointLocations()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = new ApiRoutingTemplate
                    {
                        EndpointLocations = null
                    }
                }
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsFalseForEmptyEndpointLocations()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = new ApiRoutingTemplate
                    {
                        EndpointLocations = new List<ApiEndpointLocation>()
                    }
                }
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsTrueForMatchingEndpointLocation()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = new ApiRoutingTemplate
                    {
                        EndpointLocations = new List<ApiEndpointLocation>
                        {
                            new ApiEndpointLocation()
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForMatchingEndpointLocations()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = new ApiRoutingTemplate
                    {
                        EndpointLocations = new List<ApiEndpointLocation>
                        {
                            new ApiEndpointLocation(),
                            new ApiEndpointLocation()
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestNotFound(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }
    }
}
