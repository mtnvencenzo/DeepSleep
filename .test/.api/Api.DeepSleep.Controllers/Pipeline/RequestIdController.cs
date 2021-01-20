namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class RequestIdController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        /// <summary>Initializes a new instance of the <see cref="RequestIdController"/> class.</summary>
        /// <param name="requestContextResolver">The request context resolver.</param>
        public RequestIdController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        /// <summary>Gets the content of the no.</summary>
        public void GetNoContent()
        {
        }

        /// <summary>Gets the exception.</summary>
        /// <exception cref="Exception">Specific exception for testing request id with exceptions</exception>
        public void GetException()
        {
            throw new Exception("Specific exception for testing request id with exceptions");
        }

        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        public RequestIdModel Get()
        {
            var context = this.requestContextResolver.GetContext();

            return new RequestIdModel
            {
                RequestIdentifier = context.Request.RequestIdentifier
            };
        }

        /// <summary>Gets the disabled.</summary>
        /// <returns></returns>
        public RequestIdModel GetDisabled()
        {
            var context = this.requestContextResolver.GetContext();

            return new RequestIdModel
            {
                RequestIdentifier = context.Request.RequestIdentifier
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RequestIdModel
    {
        /// <summary>Gets or sets the request identifier.</summary>
        /// <value>The request identifier.</value>
        public string RequestIdentifier { get; set; }
    }
}
