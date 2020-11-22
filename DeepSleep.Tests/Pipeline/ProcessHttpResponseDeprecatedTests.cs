namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseDeprecatedTests
    {
        [Fact]
        public async void ReturnsTrueForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpResponseDeprecated(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullRouteInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = null
            };

            var processed = await context.ProcessHttpResponseDeprecated(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullRequestConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpResponseDeprecated(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullDeprecatedConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    Deprecated = null
                }
            };

            var processed = await context.ProcessHttpResponseDeprecated(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForFalseDeprecatedConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    Deprecated = false
                }
            };

            var processed = await context.ProcessHttpResponseDeprecated(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndAddsHeaderForTrueDeprecatedConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    Deprecated = true
                }
            };

            var processed = await context.ProcessHttpResponseDeprecated(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-Deprecated");
            context.ResponseInfo.Headers[0].Value.Should().Be("true");
        }
    }
}
