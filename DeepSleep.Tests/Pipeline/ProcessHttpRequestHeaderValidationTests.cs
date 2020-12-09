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

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenNullHeaders()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Headers = null
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenEmptyHeaders()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>()
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueWhenHeaderConfigurationIsNull()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>
                    {
                        new ApiHeader { Name = "X-Header1", Value = "MyValue1" },
                        new ApiHeader { Name = "X-Header2", Value = "MyValue2" }
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    HeaderValidationConfig = null
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
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
                RequestInfo = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>
                    {
                        new ApiHeader { Name = "X-Header1", Value = "MyValue1" },
                        new ApiHeader { Name = "X-Header2", Value = "MyValue2" }
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    HeaderValidationConfig = new ApiHeaderValidationConfiguration
                    {
                        MaxHeaderLength = maxHeaderLength
                    }
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeTrue();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsFalseWhenMaxHeaderLengthIsConfiguredAndHeadersExceedLength()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Headers = new List<ApiHeader>
                    {
                        new ApiHeader { Name = "X-Header1", Value = "MyValue1111" },
                        new ApiHeader { Name = "X-Header2", Value = "MyValue2222" },
                        new ApiHeader { Name = "X-Header2", Value = "MyValue233" }
                    }
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    HeaderValidationConfig = new ApiHeaderValidationConfiguration
                    {
                        MaxHeaderLength = 10
                    }
                }
            };

            var processed = await context.ProcessHttpRequestHeaderValidation().ConfigureAwait(false);
            processed.Should().BeFalse();
            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(431);

            context.ProcessingInfo.Should().NotBeNull();
            context.ErrorMessages.Should().NotBeNull();
            context.ErrorMessages.Should().HaveCount(2);
            context.ErrorMessages[0].Should().StartWith("431.000001|");
            context.ErrorMessages[0].Should().Contain("'X-Header1'");
            context.ErrorMessages[0].Should().Contain(" exceed 10 ");
            context.ErrorMessages[1].Should().StartWith("431.000001|");
            context.ErrorMessages[1].Should().Contain("'X-Header2'");
            context.ErrorMessages[1].Should().Contain(" exceed 10 ");
        }
    }
}
