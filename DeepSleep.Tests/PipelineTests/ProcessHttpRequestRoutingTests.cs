namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestRoutingTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestRouting(null, null, null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullResover()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestRouting(new DefaultApiRoutingTable(), null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RouteInfo.TemplateInfo.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullRoutingTable()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestRouting(null, new DefaultRouteResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RouteInfo.TemplateInfo.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndNullRouteAndTemplateForEmptyRouteTable()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestRouting(new DefaultApiRoutingTable(), new DefaultRouteResolver(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.RouteInfo.Should().NotBeNull();
            context.RouteInfo.RoutingItem.Should().BeNull();
            context.RouteInfo.TemplateInfo.Should().BeNull();
        }
    }
}
