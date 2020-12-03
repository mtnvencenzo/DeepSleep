namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpConformanceTests
    {
        [Fact]
        public async void ReturnsFalseForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpConformance().ConfigureAwait(false);
            processed.Should().BeFalse();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("http/1.1")]
        [InlineData("HTTP/1.1")]
        public async void ReturnsTrueForSupportedProtocols(string protocol)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Protocol = protocol
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    HttpConfig = new ApiHttpConfiguration
                    {
                        SupportedVersions = new string[] { protocol.ToUpper() }
                    }
                }
            };

            var processed = await context.ProcessHttpConformance().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("http/1.0")]
        [InlineData("http/1.1")]
        [InlineData("HTTP/1.1")]
        [InlineData("HTTP/2.0")]
        [InlineData("HTTP/2.1")]
        [InlineData("anything")]
        public async void ReturnsTrueWhenAllProtocolsAreSupported(string protocol)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Protocol = protocol
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    HttpConfig = new ApiHttpConfiguration
                    {
                        SupportedVersions = new string[] { "*" }
                    }
                }
            };

            var processed = await context.ProcessHttpConformance().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Theory]
        [InlineData("HTTP/1.1", true)]
        [InlineData("http/1.1", true)]
        [InlineData("HTTP/2.0", true)]
        [InlineData("http/2.0", true)]
        [InlineData("HTTP/2.1", true)]
        [InlineData("http/2.1", true)]
        [InlineData("http/1.0", false)]
        [InlineData("HTTP/1.0", false)]
        public async void ReturnsFalseForUnsupportedProtocols(string protocol, bool accept)
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestInfo = new ApiRequestInfo
                {
                    Protocol = protocol
                },
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    HttpConfig = new ApiHttpConfiguration
                    {
                        SupportedVersions = new string[] { "http/1.1", "http/2.0", "http/2.1" }
                    }
                }
            };

            var processed = await context.ProcessHttpConformance().ConfigureAwait(false);
            processed.Should().Be(accept);

            if (!accept)
            {
                context.ResponseInfo.Should().NotBeNull();
                context.ResponseInfo.ResponseObject.Should().BeNull();
                context.ResponseInfo.StatusCode.Should().Be(505);
            }
            else
            {
                context.ResponseInfo.Should().NotBeNull();
                context.ResponseInfo.ResponseObject.Should().BeNull();
            }
        }
    }
}
