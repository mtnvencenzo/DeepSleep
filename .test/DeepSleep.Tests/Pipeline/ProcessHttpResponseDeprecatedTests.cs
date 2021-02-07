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
        public async void pipeline_deprecated___returns_true_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_deprecated___returns_true_and_doesnt_add_header_for_null_routeInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = null
            };

            var processed = await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_deprecated___returns_true_and_doesnt_add_header_for_null_requestConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = null
            };

            var processed = await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_deprecated___returns_true_and_doesnt_add_header_for_null_deprecatedConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DeepSleepRequestConfiguration
                {
                    Deprecated = null
                }
            };

            var processed = await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_deprecated___returns_true_and_doesnt_add_header_for_false_deprecatedConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DeepSleepRequestConfiguration
                {
                    Deprecated = false
                }
            };

            var processed = await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_deprecated___returns_true_and_adds_header_for_true_deprecatedConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DeepSleepRequestConfiguration
                {
                    Deprecated = true
                }
            };

            var processed = await context.ProcessHttpResponseDeprecated().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-Deprecated");
            context.Response.Headers[0].Value.Should().Be("true");
        }
    }
}
