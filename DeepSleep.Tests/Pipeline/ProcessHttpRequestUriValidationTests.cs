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

            var processed = await context.ProcessHttpRequestUriValidation(null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestUriValidation(null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestUriValidation(null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestUriValidation(null, null).ConfigureAwait(false);
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

            var processed = await context.ProcessHttpRequestUriValidation(new DefaultApiResponseMessageConverter(), null).ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(414);

            context.ProcessingInfo.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().HaveCount(1);
            context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("414.000001");
            context.ProcessingInfo.ExtendedMessages[0].Message.Should().Contain("exceed 2083 characters");
        }
    }
}
