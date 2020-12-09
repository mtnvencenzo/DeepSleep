namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpRequestUriValidationTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestInfoIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = null
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void ReturnsTrueWhenRequestUriDoesntExist(string requestUri)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = requestUri
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData(2083)]
        [InlineData(1)]
        public async void ReturnsTrueWhenRequestUriDoesntExceedMaxLength(int length)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = new string('a', length)
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData(2084)]
        public async void ReturnsFalseWhenRequestUriExceedsMaxLength(int length)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = new string('a', length)
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(414);

            context.ProcessingInfo.Should().NotBeNull();
            context.ErrorMessages.Should().NotBeNull();
            context.ErrorMessages.Should().HaveCount(1);
            context.ErrorMessages[0].Should().StartWith("414.000001|");
            context.ErrorMessages[0].Should().Contain("exceed 2083 characters");
        }
    }
}
