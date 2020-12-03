namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System.Threading.Tasks;
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

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullRequestInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = null
            };

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void ReturnsTrueAndDoesntAddHeaderForNullCorrelationId()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    CorrelationId = null
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
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
                RequestInfo = new ApiRequestInfo
                {
                    CorrelationId = correlationId
                }
            };

            var processed = await context.ProcessHttpResponseCorrelation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-CorrelationId");
            context.ResponseInfo.Headers[0].Value.Should().Be(correlationId);
        }
    }
}
