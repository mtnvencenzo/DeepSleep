namespace DeepSleep.Tests.PipelineTests
{
    using DeepSleep.Configuration;
    using DeepSleep.Pipeline;
    using FluentAssertions;
    using System;
    using Xunit;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessHttpResponseUnhandledExceptionTests
    {
        [Fact]
        public async void ReturnsTrueForNullException()
        {
            var context = new ApiRequestContext();

            var processed = await context.ProcessHttpResponseUnhandledException(null, null, null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndProcessNotImplementedExceptionCorrectly()
        {
            var context = new ApiRequestContext();

            var exception = new ApiNotImplementedException("Not Implmented");

            var processed = await context.ProcessHttpResponseUnhandledException(exception, null, new DefaultApiResponseMessageConverter(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(501);

            context.ProcessingInfo.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().HaveCount(1);
            context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("501.000001");
            context.ProcessingInfo.ExtendedMessages[0].Message.Should().NotBeNullOrEmpty();
            context.ProcessingInfo.Exceptions.Should().NotBeNull();
            context.ProcessingInfo.Exceptions.Should().HaveCount(1);
            context.ProcessingInfo.Exceptions[0].Should().Be(exception);
        }

        [Fact]
        public async void ReturnsTrueAndProcessNotImplementedExceptionCorrectlyAndDoesntHandleExceptionNotCalled()
        {
            var context = new ApiRequestContext();

            bool exHandled = false;

            var config = new DefaultApiServiceConfiguration
            {
                ExceptionHandler = (ctx, ex) => { exHandled = true; throw new Exception("Error"); }
            };

            var exception = new ApiNotImplementedException("Not Implmented");

            var processed = await context.ProcessHttpResponseUnhandledException(exception, config, new DefaultApiResponseMessageConverter(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(501);

            context.ProcessingInfo.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().HaveCount(1);
            context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("501.000001");
            context.ProcessingInfo.ExtendedMessages[0].Message.Should().NotBeNullOrEmpty();
            exHandled.Should().Be(false);
            context.ProcessingInfo.Exceptions.Should().NotBeNull();
            context.ProcessingInfo.Exceptions.Should().HaveCount(1);
            context.ProcessingInfo.Exceptions[0].Should().Be(exception);
        }

        [Fact]
        public async void ReturnsTrueAndProcessExceptionCorrectlyAndProcessExceptionCalled()
        {
            var context = new ApiRequestContext();

            bool exHandled = false;

            var config = new DefaultApiServiceConfiguration
            {
                ExceptionHandler = (ctx, ex) => { exHandled = true; throw new Exception("Error"); }
            };

            var exception = new Exception("ex");

            var processed = await context.ProcessHttpResponseUnhandledException(exception, config, new DefaultApiResponseMessageConverter(), null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.StatusCode.Should().Be(500);

            context.ProcessingInfo.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().NotBeNull();
            context.ProcessingInfo.ExtendedMessages.Should().HaveCount(1);
            context.ProcessingInfo.ExtendedMessages[0].Code.Should().Be("500.000001");
            context.ProcessingInfo.ExtendedMessages[0].Message.Should().NotBeNullOrEmpty();
            exHandled.Should().Be(true);
            context.ProcessingInfo.Exceptions.Should().NotBeNull();
            context.ProcessingInfo.Exceptions.Should().HaveCount(1);
            context.ProcessingInfo.Exceptions[0].Should().Be(exception);
        }
    }
}
