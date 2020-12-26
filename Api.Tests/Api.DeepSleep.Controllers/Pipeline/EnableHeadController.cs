namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;

    public class EnableHeadController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        public EnableHeadController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        public RequestHeadModel GetWithDefaultEnableHead()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        public RequestHeadModel GetWithConfiguredHeadEnabled()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        public RequestHeadModel GetWithConfiguredHeadDisbabled()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        public RequestHeadModel ExplicitHead()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        public RequestHeadModel GetWithExplicitHead()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }
    }

    public class RequestHeadModel
    {
        public bool? Configured { get; set; }
    }
}
