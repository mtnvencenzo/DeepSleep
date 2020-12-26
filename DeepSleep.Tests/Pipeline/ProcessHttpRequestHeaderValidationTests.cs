namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    public class ProcessHttpRequestHeaderValidationTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenNullHeaders()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Headers = null
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenEmptyHeaders()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>()
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenHeaderConfigurationIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>
                    {
                        new ApiHeader("X-Header1", "MyValue1"),
                        new ApiHeader("X-Header2", "MyValue2")
                    }
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    HeaderValidationConfig = null
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(null)]
        public async void ReturnsTrueWhenMaxHeaderLengthIsUnconfigured(int? maxHeaderLength)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>
                    {
                        new ApiHeader("X-Header1", "MyValue1"),
                        new ApiHeader("X-Header2", "MyValue2")
                    }
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    HeaderValidationConfig = new ApiHeaderValidationConfiguration
                    {
                        MaxHeaderLength = maxHeaderLength
                    }
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseWhenMaxHeaderLengthIsConfiguredAndHeadersExceedLength()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Request = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>
                    {
                        new ApiHeader("X-Header1", "MyValue1111"),
                        new ApiHeader("X-Header2", "MyValue2222"),
                        new ApiHeader("X-Header2", "MyValue233")
                    }
                },
                Configuration = new DefaultApiRequestConfiguration
                {
                    HeaderValidationConfig = new ApiHeaderValidationConfiguration
                    {
                        MaxHeaderLength = 10
                    }
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeFalse();
            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(431);

            context.Validation.Should().NotBeNull();
            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
        }
    }
}
