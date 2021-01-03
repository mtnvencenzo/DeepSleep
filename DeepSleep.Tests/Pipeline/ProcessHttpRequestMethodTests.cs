namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Discovery;
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
        public async void pipeline_method___returns_false_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_method___returns_true_when_route_info_is_nUll()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = null
            };

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_method___returns_true_when_template_info_is_nUll()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = null
                }
            };

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_method___returns_true_when_endpoint_locations_is_default()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null)
                }
            };

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_method___returns_true_when_endpoint_locations_is_empty()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null)
                }
            };

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_method___returns_true_when_routing_item_is_not_nUll()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null),
                    Route = new ApiRoutingItem()
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "POST" });

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void pipeline_method___returns_false_for_method_not_allowed_for_unsupported_method()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null),
                    Route = null
                },
                Request = new ApiRequestInfo
                {
                    Path = "/test/path"
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "POST" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "put" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "PATCH" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = null });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "DelEte" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "get" });


            var resolver = new DefaultRouteResolver();
            var routes = new DefaultApiRoutingTable();

            routes.AddRoute(new ApiRouteRegistration(
                template: "test/path",
                httpMethod: "GET",
                controller: typeof(Mocks.MockController),
                endpoint: nameof(Mocks.MockController.Get)));


            var processed = await context.ProcessHttpRequestMethod(routes, resolver, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(405);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("Allow");
            context.Response.Headers[0].Value.Should().Be("POST, PUT, PATCH, DELETE, GET, HEAD");
        }

        [Fact]
        public async void pipeline_method___returns_false_for_method_not_allowed_for_unsupported_method_and_doesnt_dup_head_method()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null),
                    Route = null
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "POST" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "put" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "put" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = " " });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "HeAd" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "PATCH" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = null });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "DelEte" });
            context.Routing.Template.Locations.Add(new ApiEndpointLocation { HttpMethod = "get" });

            var processed = await context.ProcessHttpRequestMethod(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(405);

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("Allow");
            context.Response.Headers[0].Value.Should().Be("POST, PUT, HEAD, PATCH, DELETE, GET");
        }
    }
}
