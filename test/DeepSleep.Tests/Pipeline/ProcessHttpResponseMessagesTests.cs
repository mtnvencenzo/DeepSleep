namespace DeepSleep.Tests.Pipeline
{
    using DeepSleep.Pipeline;
    using DeepSleep.Validation;
    using FluentAssertions;
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
                Validation = new ApiValidationInfo
                {
                    Errors = new List<string>
                    {
                        "1|test1"
                    }
                }
            };

            var processed = await context.ProcessHttpResponseMessages(new ApiRequestContextResolver()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ProcessExtendedMessagesForMultipleProviders()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Validation = new ApiValidationInfo
                {
                    Errors = new List<string>
                    {
                        "test1.0",
                        "test1.1",
                        "test1.0",
                        "test1.5",
                        "test1.3"
                    }
                },
                Response = new ApiResponseInfo
                {
                    StatusCode = 400
                },
                Configuration = new Configuration.DeepSleepRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ValidationErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages(new ApiRequestContextResolver()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeAssignableTo<IList<string>>();

            var apiResult = context.Response.ResponseObject as IList<string>;
            apiResult.Should().NotBeNull();
            apiResult.Should().HaveCount(4);
            apiResult[0].Should().Be("test1.0");
            apiResult[1].Should().Be("test1.1");
            apiResult[2].Should().Be("test1.5");
            apiResult[3].Should().Be("test1.3");

            context.Response.Headers.Should().NotBeNull();
            context.Response.Headers.Should().HaveCount(0);
        }

        [Fact]
        public async void ReturnsTrueForNullProcessingInfo()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Runtime = null,
                Configuration = new Configuration.DeepSleepRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ValidationErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages(new ApiRequestContextResolver()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNullExtendedMessages()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Validation = new ApiValidationInfo
                {
                    Errors = null
                },
                Configuration = new Configuration.DeepSleepRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ValidationErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages(new ApiRequestContextResolver()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForNoExtendedMessages()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Validation = new ApiValidationInfo
                {
                    Errors = new List<string>()
                },
                Configuration = new Configuration.DeepSleepRequestConfiguration
                {
                    ApiErrorResponseProvider = (p) => new ValidationErrorResponseProvider()
                }
            };

            var processed = await context.ProcessHttpResponseMessages(new ApiRequestContextResolver()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueForExtendedMessagesButNullResourceProvider()
        {
            var context = new ApiRequestContext
            {
                RequestAborted = new System.Threading.CancellationToken(false),
                Validation = new ApiValidationInfo
                {
                    Errors = new List<string>
                    {
                        "1|test",
                        "2|test2"
                    }
                }
            };

            var processed = await context.ProcessHttpResponseMessages(new ApiRequestContextResolver()).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }
    }
}
