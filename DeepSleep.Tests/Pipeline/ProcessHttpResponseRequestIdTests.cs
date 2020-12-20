namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseRequestIdTests
    {
        [Fact]
        public async void pipeline_requestId___returns_true_for_cancelled_request()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true)
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_null_routeInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RouteInfo = null
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_null_requestConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = null
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_null_requestIdConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = null
                }
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_false_requestIdConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = false
                }
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_empty_id_and_true_requestIdConfig(string requestId)
        {
            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    RequestIdentifier = requestId
                },
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = true
                }
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_adds_header_for_true_requestIdConfig()
        {
            var requestId = $"test-Id-{Guid.NewGuid()}";

            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo
                {
                    RequestIdentifier = requestId
                },
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = true
                }
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-RequestId");
            context.ResponseInfo.Headers[0].Value.Should().Be(requestId);
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_adds_header_using_default_id_for_true_requestIdConfig()
        {
            var context = new ApiRequestContext
            {
                RequestInfo = new ApiRequestInfo(),
                RequestAborted = new System.Threading.CancellationToken(false),
                RequestConfig = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = true
                }
            };

            var processed = await context.ProcessHttpResponseRequestId().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(1);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-RequestId");
            context.ResponseInfo.Headers[0].Value.Should().Be(context.RequestInfo.RequestIdentifier);
        }
    }
}
