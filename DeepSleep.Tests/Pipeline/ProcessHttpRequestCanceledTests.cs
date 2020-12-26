namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestCanceledTests
    {
        [Fact]
        public async void ReturnsFalseForCanceledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestCanceled().ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(408);
        }

        [Fact]
        public async void ReturnsTrueForNotCanceledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false)
            };

            var processed = await context.ProcessHttpRequestCanceled().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }
    }
}
