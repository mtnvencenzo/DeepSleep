namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
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

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseForNullRouteInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = null
            };

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsFalseForNullTemplateInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = null
                }
            };

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsFalseForDefaultEndpointLocations()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null)
                }
            };

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsFalseForEmptyEndpointLocations()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null)
                }
            };

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async void ReturnsTrueForMatchingEndpointLocation()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null)
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: null));

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForMatchingEndpointLocations()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = new ApiRoutingInfo
                {
                    Template = new ApiRoutingTemplate(null)
                }
            };

            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: null));
            context.Routing.Template.Locations.Add(new ApiEndpointLocation(controller: null, methodInfo: null, httpMethod: null));

            var processed = await context.ProcessHttpRequestNotFound().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }
    }
}
