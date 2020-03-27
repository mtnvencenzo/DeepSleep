namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestCrossOriginResourceSharingPreflightTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("post")]
        [InlineData("POST")]
        [InlineData("put")]
        [InlineData("PUT")]
        [InlineData("get")]
        [InlineData("GET")]
        [InlineData("delete")]
        [InlineData("DELETE")]
        [InlineData("patch")]
        [InlineData("PATCH")]
        [InlineData("head")]
        [InlineData("HEAD")]
        public async void ReturnsTrueForNonOptionsPreflightRequestMethod(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = method
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueForNoOriginPreflightRequestMethod(string origin)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueForNoRequestMethodPreflightRequestMethod(string requestMethod)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = requestMethod
                    }
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("post")]
        [InlineData("PUT")]
        [InlineData("delete")]
        [InlineData("PaTCh")]
        [InlineData("GET")]
        [InlineData("Head")]
        public async void ReturnsCorrectMatchedRouteTemplatesAllowMethodsForPreflightRequestMethod(string requestMethod)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = requestMethod
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = new ApiRoutingTemplate
                    {
                        EndpointLocations = new List<ApiEndpointLocation>
                        {
                            new ApiEndpointLocation{ HttpMethod = "POST" },
                            new ApiEndpointLocation{ HttpMethod = "PUT" },
                            new ApiEndpointLocation{ HttpMethod = "PUT" },
                            new ApiEndpointLocation{ HttpMethod = "PATCH" },
                            new ApiEndpointLocation{ HttpMethod = null },
                            new ApiEndpointLocation{ HttpMethod = "get" },
                            new ApiEndpointLocation{ HttpMethod = "DelEte" },
                            new ApiEndpointLocation{ HttpMethod = " " },
                            new ApiEndpointLocation{ HttpMethod = "" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(200);

            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.ResponseInfo.Headers[0].Value.Should().Be("POST, PUT, PATCH, GET, DELETE");
        }

        [Fact]
        public async void ReturnsCorrectRequestHeadersForPreflightRequestMethod()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = "POST",
                        AccessControlRequestHeaders = "Content-Type, X-Header"
                    }
                },
                RouteInfo = new ApiRoutingInfo
                {
                    TemplateInfo = new ApiRoutingTemplate
                    {
                        EndpointLocations = new List<ApiEndpointLocation>
                        {
                            new ApiEndpointLocation{ HttpMethod = "POST" }
                        }
                    }
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(200);

            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(2);
            context.ResponseInfo.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.ResponseInfo.Headers[0].Value.Should().Be("POST");
            context.ResponseInfo.Headers[1].Name.Should().Be("Access-Control-Allow-Headers");
            context.ResponseInfo.Headers[1].Value.Should().Be("Content-Type, X-Header");
        }
    }
}
