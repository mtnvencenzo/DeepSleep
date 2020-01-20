namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestMethodTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestMethod ().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRouteInfoIsNUll()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = null
            };

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenTemplateInfoIsNUll()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = null
                }
            };

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenEndpointLocationsIsNUll()
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

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenEndpointLocationsIsEmpty()
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

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRoutingItemIsNotNUll()
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
                            new ApiEndpointLocation { HttpMethod = "POST" }
                        }
                    },
                    RoutingItem = new ApiRoutingItem
                    {
                    }
                }
            };

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }


        [Fact]
        public async void ReturnsFalseForMethodNotAllowedForUnsupportedMethod()
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
                            new ApiEndpointLocation { HttpMethod = "POST" },
                            new ApiEndpointLocation { HttpMethod = "put" },
                            new ApiEndpointLocation { HttpMethod = "" },
                            new ApiEndpointLocation { HttpMethod = "PATCH" },
                            new ApiEndpointLocation { HttpMethod = null },
                            new ApiEndpointLocation { HttpMethod = "DelEte" },
                            new ApiEndpointLocation { HttpMethod = "get" }
                        }
                    },
                    RoutingItem = null
                }
            };

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(405);

            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("Allow");
            context.ResponseInfo.Headers[0].Value.Should().Be("POST, PUT, PATCH, DELETE, GET, HEAD");
        }

        [Fact]
        public async void ReturnsFalseForMethodNotAllowedForUnsupportedMethodAndDoesntDupHeadMethod()
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
                            new ApiEndpointLocation { HttpMethod = "POST" },
                            new ApiEndpointLocation { HttpMethod = "put" },
                            new ApiEndpointLocation { HttpMethod = "put" },
                            new ApiEndpointLocation { HttpMethod = " " },
                            new ApiEndpointLocation { HttpMethod = "HeAd" },
                            new ApiEndpointLocation { HttpMethod = "" },
                            new ApiEndpointLocation { HttpMethod = "PATCH" },
                            new ApiEndpointLocation { HttpMethod = null },
                            new ApiEndpointLocation { HttpMethod = "DelEte" },
                            new ApiEndpointLocation { HttpMethod = "get" }
                        }
                    },
                    RoutingItem = null
                }
            };

            var processed = await context.ProcessHttpRequestMethod().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(405);

            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("Allow");
            context.ResponseInfo.Headers[0].Value.Should().Be("POST, PUT, HEAD, PATCH, DELETE, GET");
        }
    }
}
