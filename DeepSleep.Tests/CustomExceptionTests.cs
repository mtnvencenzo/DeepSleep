namespace DeepSleep.Tests
{
    using FluentAssertions;
    using System;
    using Xunit;

    public class CustomExceptionTests
    {
        [Fact]
        public void exceptions___api_not_implmented_exception_ctor_test()
        {
            var innerException = new Exception("Inner Exception");
            var customMessage = "My Custom Exception";
            var expectedHttpStatusCode = 501;

            var ex = new ApiNotImplementedException();
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be($"Exception of type '{typeof(ApiNotImplementedException).FullName}' was thrown.");
            ex.InnerException.Should().BeNull();

            ex = new ApiNotImplementedException(customMessage);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeNull();

            ex = new ApiNotImplementedException(customMessage, innerException);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeSameAs(innerException);
        }

        [Fact]
        public void exceptions___api_bad_gateway_exception_ctor_test()
        {
            var innerException = new Exception("Inner Exception");
            var customMessage = "My Custom Exception";
            var expectedHttpStatusCode = 502;

            var ex = new ApiBadGatewayException();
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be($"Exception of type '{typeof(ApiBadGatewayException).FullName}' was thrown.");
            ex.InnerException.Should().BeNull();

            ex = new ApiBadGatewayException(customMessage);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeNull();

            ex = new ApiBadGatewayException(customMessage, innerException);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeSameAs(innerException);
        }

        [Fact]
        public void exceptions___api_gateway_timeout_exception_ctor_test()
        {
            var innerException = new Exception("Inner Exception");
            var customMessage = "My Custom Exception";
            var expectedHttpStatusCode = 504;

            var ex = new ApiGatewayTimeoutException();
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be($"Exception of type '{typeof(ApiGatewayTimeoutException).FullName}' was thrown.");
            ex.InnerException.Should().BeNull();

            ex = new ApiGatewayTimeoutException(customMessage);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeNull();

            ex = new ApiGatewayTimeoutException(customMessage, innerException);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeSameAs(innerException);
        }

        [Fact]
        public void exceptions___api_service_unavailable_exception_ctor_test()
        {
            var innerException = new Exception("Inner Exception");
            var customMessage = "My Custom Exception";
            var expectedHttpStatusCode = 503;

            var ex = new ApiServiceUnavailableException();
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be($"Exception of type '{typeof(ApiServiceUnavailableException).FullName}' was thrown.");
            ex.InnerException.Should().BeNull();

            ex = new ApiServiceUnavailableException(customMessage);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeNull();

            ex = new ApiServiceUnavailableException(customMessage, innerException);
            ex.HttpStatus.Should().Be(expectedHttpStatusCode);
            ex.Message.Should().Be(customMessage);
            ex.InnerException.Should().BeSameAs(innerException);
        }
    }
}
