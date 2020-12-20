namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
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
                RequestInfo = null,
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = 1
                }
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
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = 1
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestUriDoesntExceedMaxLength()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = url
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = url.Length
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestUriMaxLengthNull()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = url
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = null
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestUriMaxLengthNegative()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = url
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = -1
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }


        [Fact]
        public async void ReturnsTrueWhenRequestUriMaxLengthZero()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = url
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = 0
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestConfigIsNull()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = url
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = 0
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseWhenRequestUriExceedsMaxLength()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    RequestUri = url
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    MaxRequestUriLength = url.Length - 1
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(414);

            context.ProcessingInfo.Should().NotBeNull();
            context.ErrorMessages.Should().NotBeNull();
            context.ErrorMessages.Should().HaveCount(0);
        }
    }
}
