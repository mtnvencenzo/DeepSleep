namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System.Threading;
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
                RequestAborted = new CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
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
        public async void pipeline_preflight__returns_true_for_non_options_preflight_request_method(string method)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = method
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void pipeline_preflight__returns_true_for_no_origin_preflight_request_method(string origin)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = origin
                    }
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void pipeline_preflight__returns_true_for_no_request_method_preflight_request_method(string requestMethod)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = requestMethod
                    }
                }
            };

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("post")]
        [InlineData("PUT")]
        [InlineData("delete")]
        [InlineData("PaTCh")]
        [InlineData("GET")]
        [InlineData("Head")]
        public async void pipeline_preflight__returns_correct_matched_route_templates_allow_methods_for_preflight_request_method(string requestMethod)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = requestMethod
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate("/test")
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "POST"));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "PUT"));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "PUT"));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "PATCH"));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: null));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "get"));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "DelEte"));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: " "));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: ""));


            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.Response.Headers[0].Value.Should().Be("POST, PUT, PATCH, GET, DELETE");
        }

        [Fact]
        public async void pipeline_preflight__returns_correct_request_headers_for_preflight_request_method_with_no_config()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = "POST",
                        AccessControlRequestHeaders = "Content-Type, X-Header"
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate("/test")
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "POST"));


            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.Response.Headers[0].Value.Should().Be("POST");
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Headers");
            context.Response.Headers[1].Value.Should().BeEmpty();
        }

        [Theory]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("", " ")]
        [InlineData("Content-Type", "Content-Type")]
        [InlineData("X-BeansWithBacon, PorkNBeans", "X-BeansWithBacon", "PorkNBeans")]
        [InlineData("Content-Type, X-Header", "*", "X-Header")]
        [InlineData("Content-Type, X-Header", "*")]
        public async void pipeline_preflight__returns_correct_request_headers_for_preflight_request_method(string expectedAllowHeaders, params string[] requestHeaders)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = "POST",
                        AccessControlRequestHeaders = "Content-Type, X-Header"
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate("/test")
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedHeaders = requestHeaders
                    }
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "POST"));

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.Response.Headers[0].Value.Should().Be("POST");
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Headers");
            context.Response.Headers[1].Value.Should().Be(expectedAllowHeaders);
        }

        [Theory]
        [InlineData(200)]
        [InlineData(201)]
        [InlineData(202)]
        [InlineData(203)]
        [InlineData(299)]
        public async void pipeline_preflight__returns_true_and_correct_headers_for_preflight_request(int statusCode)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = "POST",
                        AccessControlRequestHeaders = "Content-Type, X-Header"
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate("/test")
                },

                Response = new ApiResponseInfo
                {
                    StatusCode = statusCode
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedHeaders = new string[] { "Content-Type" }
                    }
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "POST"));

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(2);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.Response.Headers[0].Value.Should().Be("POST");
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Headers");
            context.Response.Headers[1].Value.Should().Be("Content-Type");
        }

        [Fact]
        public async void pipeline_preflight__returns_true_and_includes_max_age_for_explicit_config()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Method = "options",
                    CrossOriginRequest = new CrossOriginRequestValues
                    {
                        Origin = "http://ron.vecchi.net",
                        AccessControlRequestMethod = "POST",
                        AccessControlRequestHeaders = "Content-Type, X-Header"
                    }
                },
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate("/test")
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    CrossOriginConfig = new ApiCrossOriginConfiguration
                    {
                        AllowedHeaders = new string[] { "Content-Type", "X-RequestId" },
                        MaxAgeSeconds = 100
                    }
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: "POST"));

            var processed = await context.ProcessHttpRequestCrossOriginResourceSharingPreflight(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(200);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(3);
            context.Response.Headers[0].Name.Should().Be("Access-Control-Allow-Methods");
            context.Response.Headers[0].Value.Should().Be("POST");
            context.Response.Headers[1].Name.Should().Be("Access-Control-Allow-Headers");
            context.Response.Headers[1].Value.Should().Be("Content-Type, X-RequestId");
            context.Response.Headers[2].Name.Should().Be("Access-Control-Max-Age");
            context.Response.Headers[2].Value.Should().Be("100");
        }
    }
}
