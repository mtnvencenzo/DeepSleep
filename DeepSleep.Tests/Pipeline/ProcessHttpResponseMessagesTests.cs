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
                ProcessingInfo = new ApiProcessingInfo
                {
                    ExtendedMessages = new List<ApiResponseMessage>
                    {
                        new ApiResponseMessage{ Code = "1", Message = "test1" }
                    }
                }
            };

            var provider = new Mock<IApiResponseMessageProcessorProvider>();
            provider
                .Setup(m => m.GetProcessors())
                .Throws(new System.Exception("test"));

            var processed = await context.ProcessHttpResponseMessages(provider.Object).ConfigureAwait(false);
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
                ProcessingInfo = new ApiProcessingInfo
                {
                    ExtendedMessages = new List<ApiResponseMessage>
                    {
                        new ApiResponseMessage{ Code = "200", Message = "test1" },
                        new ApiResponseMessage{ Code = "100", Message = "test1" },
                        new ApiResponseMessage{ Code = "100", Message = "test1" },
                        new ApiResponseMessage{ Code = "500", Message = "test1" },
                        new ApiResponseMessage{ Code = "300", Message = "test1" },
                    }
                },
                ResponseInfo = new ApiResponseInfo
                {
                    StatusCode = 400
                }
            };

            var provider = new DefaultApiResponseMessageProcessorProvider(null)
                .RegisterProcessor<ApiResultResponseMessageProcessor>()
                .RegisterProcessor<HttpHeaderResponseMessageProcessor>();

            var processed = await context.ProcessHttpResponseMessages(provider).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeAssignableTo<ApiResult>();

            var apiResult = context.ResponseInfo.ResponseObject as ApiResult;
            apiResult.Should().NotBeNull();
            apiResult.Messages.Should().NotBeNull();
            apiResult.Messages.Should().HaveCount(4);
            apiResult.Messages[0].Code.Should().Be("100");
            apiResult.Messages[0].Message.Should().Be("test1");
            apiResult.Messages[1].Code.Should().Be("200");
            apiResult.Messages[1].Message.Should().Be("test1");
            apiResult.Messages[2].Code.Should().Be("300");
            apiResult.Messages[2].Message.Should().Be("test1");
            apiResult.Messages[3].Code.Should().Be("500");
            apiResult.Messages[3].Message.Should().Be("test1");

            context.ResponseInfo.Headers.Should().NotBeNull();
            context.ResponseInfo.Headers.Should().HaveCount(4);
            context.ResponseInfo.Headers[0].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[0].Value.Should().Be("100|test1");
            context.ResponseInfo.Headers[1].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[1].Value.Should().Be("200|test1");
            context.ResponseInfo.Headers[2].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[2].Value.Should().Be("300|test1");
            context.ResponseInfo.Headers[3].Name.Should().Be("X-Api-Message");
            context.ResponseInfo.Headers[3].Value.Should().Be("500|test1");
        }

        [Fact]
        public async void ReturnsTrueForNullProcessingInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                ProcessingInfo = null
            };

            var provider = new Mock<IApiResponseMessageProcessorProvider>();
            provider
                .Setup(m => m.GetProcessors())
                .Throws(new System.Exception("test"));

            var processed = await context.ProcessHttpResponseMessages(provider.Object).ConfigureAwait(false);
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
                ProcessingInfo = new ApiProcessingInfo
                {
                    ExtendedMessages = null
                }
            };

            var provider = new Mock<IApiResponseMessageProcessorProvider>();
            provider
                .Setup(m => m.GetProcessors())
                .Throws(new System.Exception("test"));

            var processed = await context.ProcessHttpResponseMessages(provider.Object).ConfigureAwait(false);
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
                ProcessingInfo = new ApiProcessingInfo
                {
                    ExtendedMessages = new List<ApiResponseMessage>()
                }
            };

            var provider = new Mock<IApiResponseMessageProcessorProvider>();
            provider
                .Setup(m => m.GetProcessors())
                .Throws(new System.Exception("test"));

            var processed = await context.ProcessHttpResponseMessages(provider.Object).ConfigureAwait(false);
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
                ProcessingInfo = new ApiProcessingInfo
                {
                    ExtendedMessages = new List<ApiResponseMessage>
                    {
                        new ApiResponseMessage{ Code = "1", Message = "test" },
                        new ApiResponseMessage{ Code = "2", Message = "test2" }
                    }
                }
            };

            var processed = await context.ProcessHttpResponseMessages(null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }
    }
}
