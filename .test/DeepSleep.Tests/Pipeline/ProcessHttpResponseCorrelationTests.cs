namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseCorrelationTests
    {
        [Fact]
        public async void ReturnsTrueForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpResponseCorrelation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullRequestInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = null,
                Configuration = new DeepSleepRequestConfiguration
                {
                    UseCorrelationIdHeader = true
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullCorrelationId()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    CorrelationId = null
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    UseCorrelationIdHeader = true
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" this is my 1@537649* )()*& Correlation id ")]
        public async void ReturnsTrueAndAddsHeaderMatchingCorrelationId(string correlationId)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    CorrelationId = correlationId
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    UseCorrelationIdHeader = true
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-CorrelationId");
            context.Response.Headers[0].Value.Should().Be(correlationId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" this is my 1@537649* )()*& Correlation id ")]
        public async void ReturnsTrueAndAddsHeaderMatchingCorrelationIdDefaultSupported(string correlationId)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    CorrelationId = correlationId
                }
            };

            var defaultConfig = new DeepSleepRequestConfiguration
            {
                UseCorrelationIdHeader = true
            };

            var processed = await context.ProcessHttpResponseCorrelation(defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-CorrelationId");
            context.Response.Headers[0].Value.Should().Be(correlationId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" this is my 1@537649* )()*& Correlation id ")]
        public async void ReturnsTrueAndAddsHeaderMatchingCorrelationIdSystemSupported(string correlationId)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    CorrelationId = correlationId
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-CorrelationId");
            context.Response.Headers[0].Value.Should().Be(correlationId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" this is my 1@537649* )()*& Correlation id ")]
        public async void ReturnsTrueAndDoesNotAddHeaderMatchingCorrelationIdEndpointNotSupported(string correlationId)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    CorrelationId = correlationId
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    UseCorrelationIdHeader = false
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" this is my 1@537649* )()*& Correlation id ")]
        public async void ReturnsTrueAndDoesNotAddHeaderMatchingCorrelationIdDefaultNotSupported(string correlationId)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    CorrelationId = correlationId
                }
            };

            var defaultConfig = new DeepSleepRequestConfiguration
            {
                UseCorrelationIdHeader = false
            };

            var processed = await context.ProcessHttpResponseCorrelation(defaultConfig).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }
    }
}
