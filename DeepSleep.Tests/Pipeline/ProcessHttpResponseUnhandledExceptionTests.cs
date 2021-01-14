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
        public async void unhandled_exception___returns_true_for_null_exception()
        {
            var context = new ApiRequestContext();

            var processed = await context.ProcessHttpResponseUnhandledException(new DefaultApiRequestContextResolver(), null, null).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
        }

        [Fact]
        public async void unhandled_exception___returns_true_and_process_not_implemented_exception_correctly()
        {
            var context = new ApiRequestContext();

            var exception = new ApiNotImplementedException();

            var processed = await context.ProcessHttpResponseUnhandledException(new DefaultApiRequestContextResolver(), exception, null).ConfigureAwait(false);
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
        public async void unhandled_exception___returns_true_and_process_not_implemented_exception_handler_not_called()
        {
            var context = new ApiRequestContext();

            bool exHandled = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnException = (ctx, ex) => { exHandled = true; throw new Exception("Error"); }
            };

            var exception = new ApiNotImplementedException();

            var processed = await context.ProcessHttpResponseUnhandledException(new DefaultApiRequestContextResolver(), exception, config).ConfigureAwait(false);
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
        public async void unhandled_exception___returns_true_and_process_bad_gateway_exception_handler_not_called()
        {
            var context = new ApiRequestContext();

            bool exHandled = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnException = (ctx, ex) => { exHandled = true; throw new Exception("Error"); }
            };

            var exception = new ApiBadGatewayException();

            var processed = await context.ProcessHttpResponseUnhandledException(new DefaultApiRequestContextResolver(), exception, config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(502);

            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
            exHandled.Should().Be(false);
            context.Runtime.Exceptions.Should().NotBeNull();
            context.Runtime.Exceptions.Should().HaveCount(1);
            context.Runtime.Exceptions[0].Should().Be(exception);
        }

        [Fact]
        public async void unhandled_exception___returns_true_and_process_service_unavailable_exception_handler_not_called()
        {
            var context = new ApiRequestContext();

            bool exHandled = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnException = (ctx, ex) => { exHandled = true; throw new Exception("Error"); }
            };

            var exception = new ApiServiceUnavailableException();

            var processed = await context.ProcessHttpResponseUnhandledException(new DefaultApiRequestContextResolver(), exception, config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(503);

            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
            exHandled.Should().Be(false);
            context.Runtime.Exceptions.Should().NotBeNull();
            context.Runtime.Exceptions.Should().HaveCount(1);
            context.Runtime.Exceptions[0].Should().Be(exception);
        }

        [Fact]
        public async void unhandled_exception___returns_true_and_process_gateway_timeout_exception_handler_not_called()
        {
            var context = new ApiRequestContext();

            bool exHandled = false;

            var config = new DefaultApiServiceConfiguration
            {
                OnException = (ctx, ex) => { exHandled = true; throw new Exception("Error"); }
            };

            var exception = new ApiGatewayTimeoutException();

            var processed = await context.ProcessHttpResponseUnhandledException(new DefaultApiRequestContextResolver(), exception, config).ConfigureAwait(false);
            processed.Should().BeTrue();

            context.Response.Should().NotBeNull();
            context.Response.ResponseObject.Should().BeNull();
            context.Response.StatusCode.Should().Be(504);

            context.Validation.Errors.Should().NotBeNull();
            context.Validation.Errors.Should().HaveCount(0);
            exHandled.Should().Be(false);
            context.Runtime.Exceptions.Should().NotBeNull();
            context.Runtime.Exceptions.Should().HaveCount(1);
            context.Runtime.Exceptions[0].Should().Be(exception);
        }
    }
}
