namespace DeepSleep.Tests.Pipeline
{
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

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
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
                Request = null
            };

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
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
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
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
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-CorrelationId");
            context.Response.Headers[0].Value.Should().Be(correlationId);
        }
    }
}
