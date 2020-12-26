namespace DeepSleep.Tests.Pipeline
{
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

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void ReturnsTrueAndProcessNotImplementedExceptionCorrectly()
        {
            var context = new ApiRequestContext();

            var exception = new ApiNotImplementedException();

            var processed = await context.ProcessHttpResponseUnhandledException(exception, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(501);

            context.Runtime.Should().NotBeNull();
            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
            context.Runtime.Exceptions.Should().NotBeNull();
            context.Runtime.Exceptions.Should().HaveCount(1);
            context.Runtime.Exceptions[0].Should().Be(exception);
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

            var exception = new ApiNotImplementedException();

            var processed = await context.ProcessHttpResponseUnhandledException(exception, config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(501);

            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
            exHandled.Should().Be(false);
            context.Runtime.Exceptions.Should().NotBeNull();
            context.Runtime.Exceptions.Should().HaveCount(1);
            context.Runtime.Exceptions[0].Should().Be(exception);
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

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(500);

            context.Validation.Should().NotBeNull();
            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
            exHandled.Should().Be(true);
            context.Runtime.Exceptions.Should().NotBeNull();
            context.Runtime.Exceptions.Should().HaveCount(1);
            context.Runtime.Exceptions[0].Should().Be(exception);
        }
    }
}
