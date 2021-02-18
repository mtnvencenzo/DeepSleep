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

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestInfoIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = null,
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = 1
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
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
                Request = new ApiRequestInfo
                {
                    RequestUri = requestUri
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = 1
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestUriDoesntExceedMaxLength()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    RequestUri = url
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = url.Length
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestUriMaxLengthNull()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    RequestUri = url
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = null
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestUriMaxLengthNegative()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    RequestUri = url
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = -1
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }


        [Fact]
        public async void ReturnsTrueWhenRequestUriMaxLengthZero()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    RequestUri = url
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = 0
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenRequestConfigIsNull()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    RequestUri = url
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = 0
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseWhenRequestUriExceedsMaxLength()
        {
            var url = "http://deepsleep.io/test/uri";

            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    RequestUri = url
                },
                Configuration = new DeepSleepRequestConfiguration
                {
                    RequestValidation = new ApiRequestValidationConfiguration
                    {
                        MaxRequestUriLength = url.Length - 1
                    }
                }
            };

            var processed = await context.ProcessHttpRequestUriValidation().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(414);

            context.Runtime.Should().NotBeNull();
            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
        }
    }
}
