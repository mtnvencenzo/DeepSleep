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
    public class ProcessHttpResponseUnhandledExceptionTests
    {
        [Fact]
        public async void ReturnsTrueForNullException()
        {
            var context = new ApiRequestContext();

            var processed = await context.ProcessHttpResponseUnhandledException(null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndProcessNotImplementedExceptionCorrectly()
        {
            var context = new ApiRequestContext();

            var exception = new ApiNotImplementedException("Not Implmented");

            var processed = await context.ProcessHttpResponseUnhandledException(exception, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(501);

            context.ProcessingInfo.Should().NotBeNull();
            context.ErrorMessages.Should().NotBeNull();
            context.ErrorMessages.Should().HaveCount(1);
            context.ErrorMessages[0].Should().StartWith("501.000001|");
            context.ErrorMessages[0].Should().NotBe("501.000001|");
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

            var processed = await context.ProcessHttpResponseUnhandledException(exception, config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(501);

            context.ErrorMessages.Should().NotBeNull();
            context.ErrorMessages.Should().HaveCount(1);
            context.ErrorMessages[0].Should().StartWith("501.000001|");
            context.ErrorMessages[0].Should().NotBe("501.000001|");
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

            var processed = await context.ProcessHttpResponseUnhandledException(exception, config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.ResponseInfo.Should().NotBeNull();
            context.ResponseInfo.ResponseObject.Should().BeNull();
            context.ResponseInfo.StatusCode.Should().Be(500);

            context.ProcessingInfo.Should().NotBeNull();
            context.ErrorMessages.Should().NotBeNull();
            context.ErrorMessages.Should().HaveCount(1);
            context.ErrorMessages[0].Should().StartWith("500.000001|");
            context.ErrorMessages[0].Should().NotBe("500.000001|");
            exHandled.Should().Be(true);
            context.ProcessingInfo.Exceptions.Should().NotBeNull();
            context.ProcessingInfo.Exceptions.Should().HaveCount(1);
            context.ProcessingInfo.Exceptions[0].Should().Be(exception);
        }
    }
}
