namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using Moq;
    using System.Collections.Generic;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseMessagesTests
    {
        [Fact]
        public async void ReturnsTrueForCancelledRequest()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(true),
                ErrorMessages = new List<string>
                {
                    "1|test1"
                }
            };

            var processed = await context.ProcessHttpResponseMessages().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ProcessExtendedMessagesForMultipleProviders()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ErrorMessages = new List<string>
                {
                    "200|test1",
                    "100|test1",
                    "100|test1",
                    "500|test1",
                    "300|test1"
                },
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 400
                },
                RequestConfig = new Configuration.DefaultApiRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ApiResultErrorResponseProvider
                    {
                        WriteToBody = true,
                        WriteToHeaders = true
                    }
                }
            };

            var processed = await context.ProcessHttpResponseMessages().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeAssignableTo<ApiResult>();

            var apiResult = context.ResponseInfo.ResponseObject as ApiResult;
            apiResult.Should().NotBeNull();
            apiResult.Messages.Should().NotBeNull();
            apiResult.Messages.Should().HaveCount(4);
            apiResult.Messages[1].Code.Should().Be("100");
            apiResult.Messages[1].Message.Should().Be("test1");
            apiResult.Messages[0].Code.Should().Be("200");
            apiResult.Messages[0].Message.Should().Be("test1");
            apiResult.Messages[3].Code.Should().Be("300");
            apiResult.Messages[3].Message.Should().Be("test1");
            apiResult.Messages[2].Code.Should().Be("500");
            apiResult.Messages[2].Message.Should().Be("test1");

            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(4);
            context.ResponseInfo.Headers[1].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[1].Value.Should().Be("100|test1");
            context.ResponseInfo.Headers[0].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[0].Value.Should().Be("200|test1");
            context.ResponseInfo.Headers[3].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[3].Value.Should().Be("300|test1");
            context.ResponseInfo.Headers[2].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[2].Value.Should().Be("500|test1");
        }

        [Fact]
        public async void ReturnsTrueForNullProcessingInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ProcessingInfo = null,
                RequestConfig = new Configuration.DefaultApiRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ApiResultErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullExtendedMessages()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ErrorMessages = null,
                RequestConfig = new Configuration.DefaultApiRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ApiResultErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNoExtendedMessages()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ErrorMessages = new List<string>(),
                RequestConfig = new Configuration.DefaultApiRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ApiResultErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForExtendedMessagesButNullResourceProvider()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ErrorMessages = new List<string>
                {
                    "1|test",
                    "2|test2"
                }
            };

            var processed = await context.ProcessHttpResponseMessages().ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }
    }
}
