namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RequestIdController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        public RequestIdController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        public void GetNoContent()
        {
        }

        public void GetException()
        {
            throw new Exception("Specific exception for testing request id with exceptions");
        }

        public RequestIdModel Get()
        {
            var context = this.requestContextResolver.GetContext();

            return new RequestIdModel
            {
                RequestIdentifier = context.Request.RequestIdentifier
            };
        }

        public RequestIdModel GetDisabled()
        {
            var context = this.requestContextResolver.GetContext();

            return new RequestIdModel
            {
                RequestIdentifier = context.Request.RequestIdentifier
            };
        }
    }

    public class RequestIdModel
    {
        public string RequestIdentifier { get; set; }
    }
}
