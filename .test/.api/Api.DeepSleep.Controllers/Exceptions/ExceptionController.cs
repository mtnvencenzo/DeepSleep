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


        /// <summary>Nots the implemented.</summary>
        /// <exception cref="ApiNotImplementedException"></exception>
        public void NotImplemented()
        {
            throw new ApiNotImplementedException();
        }

        /// <summary>Nots the implemented from validator.</summary>
        [ApiEndpointValidation(typeof(NotImplementedExceptionThrowValidator))]
        public void NotImplementedFromValidator()
        {
        }

        /// <summary>Nots the implemented from authentication.</summary>
        public void NotImplementedFromAuthentication()
        {
        }

        /// <summary>Nots the implemented from authorization.</summary>
        public void NotImplementedFromAuthorization()
        {
        }


        // 502 Bad Gateway
        // --------------------


        /// <summary>Bads the gateway.</summary>
        /// <exception cref="ApiBadGatewayException"></exception>
        public void BadGateway()
        {
            throw new ApiBadGatewayException();
        }

        /// <summary>Bads the gateway from validator.</summary>
        [ApiEndpointValidation(typeof(BadGatewayExceptionThrowValidator))]
        public void BadGatewayFromValidator()
        {
        }

        /// <summary>Bads the gateway from authentication.</summary>
        public void BadGatewayFromAuthentication()
        {
        }

        /// <summary>Bads the gateway from authorization.</summary>
        public void BadGatewayFromAuthorization()
        {
        }


        // 504 Gateway Timeout
        // --------------------


        /// <summary>Gateways the timeout.</summary>
        /// <exception cref="ApiGatewayTimeoutException"></exception>
        public void GatewayTimeout()
        {
            throw new ApiGatewayTimeoutException();
        }

        /// <summary>Gateways the timeout from validator.</summary>
        [ApiEndpointValidation(typeof(GatewayTimeoutExceptionThrowValidator))]
        public void GatewayTimeoutFromValidator()
        {
        }

        /// <summary>Gateways the timeout from authentication.</summary>
        public void GatewayTimeoutFromAuthentication()
        {
        }

        /// <summary>Gateways the timeout from authorization.</summary>
        public void GatewayTimeoutFromAuthorization()
        {
        }


        // 503 Service Unavailable
        // --------------------


        /// <summary>Services the unavailable.</summary>
        /// <exception cref="ApiServiceUnavailableException"></exception>
        public void ServiceUnavailable()
        {
            throw new ApiServiceUnavailableException();
        }

        /// <summary>Services the unavailable from validator.</summary>
        [ApiEndpointValidation(typeof(ServiceUnavailableExceptionThrowValidator))]
        public void ServiceUnavailableFromValidator()
        {
        }

        /// <summary>Services the unavailable from authentication.</summary>
        public void ServiceUnavailableFromAuthentication()
        {
        }

        /// <summary>Services the unavailable from authorization.</summary>
        public void ServiceUnavailableFromAuthorization()
        {
        }


        // 500 Unhandled
        // --------------------


        /// <summary>Unhandleds this instance.</summary>
        /// <exception cref="Exception"></exception>
        public void Unhandled()
        {
            throw new Exception();
        }

        /// <summary>Unhandleds from validator.</summary>
        [ApiEndpointValidation(typeof(UnhandledExceptionThrowValidator))]
        public void UnhandledFromValidator()
        {
        }

        /// <summary>Unhandleds from authentication.</summary>
        public void UnhandledFromAuthentication()
        {
        }

        /// <summary>Unhandleds from authorization.</summary>
        /// <returns></returns>
        public void UnhandledFromAuthorization()
        {
        }
    }
}
