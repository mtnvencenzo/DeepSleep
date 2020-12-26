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

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_adds_header_for_null_routeInfo()
        {
            var requestId = $"test-Id-{Guid.NewGuid()}";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    RequestIdentifier = requestId
                },
                RequestAborted = new System.Threading.CancellationToken(false),
                Routing = null
            };

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-RequestId");
            context.Response.Headers[0].Value.Should().Be(requestId);
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_adds_header_for_null_requestConfig()
        {
            var requestId = $"test-Id-{Guid.NewGuid()}";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    RequestIdentifier = requestId
                },
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = null
            };

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-RequestId");
            context.Response.Headers[0].Value.Should().Be(requestId);
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_null_requestId()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = null
                }
            };

            context.Request.RequestIdentifier = null;

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_false_requestIdConfig()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = false
                }
            };

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void pipeline_requestId___returns_true_and_doesnt_add_header_for_empty_id_and_true_requestIdConfig(string requestId)
        {
            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    RequestIdentifier = requestId
                },
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = true
                }
            };

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().BeEmpty();
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_adds_header_for_true_requestIdConfig()
        {
            var requestId = $"test-Id-{Guid.NewGuid()}";

            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo
                {
                    RequestIdentifier = requestId
                },
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = true
                }
            };

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-RequestId");
            context.Response.Headers[0].Value.Should().Be(requestId);
        }

        [Fact]
        public async void pipeline_requestId___returns_true_and_adds_header_using_default_id_for_true_requestIdConfig()
        {
            var context = new ApiRequestContext
            {
                Request = new ApiRequestInfo(),
                RequestAborted = new System.Threading.CancellationToken(false),
                Configuration = new DefaultApiRequestConfiguration
                {
                    IncludeRequestIdHeaderInResponse = true
                }
            };

            var processed = await context.ProcessHttpResponseRequestId(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(1);
            context.Response.Headers[0].Name.Should().Be("X-RequestId");
            context.Response.Headers[0].Value.Should().Be(context.Request.RequestIdentifier);
        }
    }
}
