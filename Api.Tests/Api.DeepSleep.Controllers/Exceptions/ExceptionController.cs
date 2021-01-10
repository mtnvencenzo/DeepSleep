namespace Api.DeepSleep.Controllers.Exceptions
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ExceptionController
    {
        // 501 Not Implemented
        // --------------------


        public void NotImplemented()
        {
            throw new ApiNotImplementedException();
        }

        [ApiEndpointValidation(typeof(NotImplementedExceptionThrowValidator))]
        public void NotImplementedFromValidator()
        {
        }

        public void NotImplementedFromAuthentication()
        {
        }

        public void NotImplementedFromAuthorization()
        {
        }


        // 502 Bad Gateway
        // --------------------


        public void BadGateway()
        {
            throw new ApiBadGatewayException();
        }

        [ApiEndpointValidation(typeof(BadGatewayExceptionThrowValidator))]
        public void BadGatewayFromValidator()
        {
        }

        public void BadGatewayFromAuthentication()
        {
        }

        public void BadGatewayFromAuthorization()
        {
        }


        // 504 Gateway Timeout
        // --------------------


        public void GatewayTimeout()
        {
            throw new ApiGatewayTimeoutException();
        }

        [ApiEndpointValidation(typeof(GatewayTimeoutExceptionThrowValidator))]
        public void GatewayTimeoutFromValidator()
        {
        }

        public void GatewayTimeoutFromAuthentication()
        {
        }

        public void GatewayTimeoutFromAuthorization()
        {
        }


        // 503 Service Unavailable
        // --------------------


        public void ServiceUnavailable()
        {
            throw new ApiServiceUnavailableException();
        }

        [ApiEndpointValidation(typeof(ServiceUnavailableExceptionThrowValidator))]
        public void ServiceUnavailableFromValidator()
        {
        }

        public void ServiceUnavailableFromAuthentication()
        {
        }

        public void ServiceUnavailableFromAuthorization()
        {
        }


        // 500 Unhandled
        // --------------------


        public void Unhandled()
        {
            throw new Exception();
        }

        [ApiEndpointValidation(typeof(UnhandledExceptionThrowValidator))]
        public void UnhandledFromValidator()
        {
        }

        public void UnhandledFromAuthentication()
        {
        }

        public void UnhandledFromAuthorization()
        {
        }
    }
}
