namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;

    public class MethodNotFoundController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        public MethodNotFoundController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        public MethodNotFoundModel Get()
        {
            return new MethodNotFoundModel
            {
                RequestMethod = requestContextResolver.GetContext().Request.Method
            };
        }

        public MethodNotFoundModel Put(MethodNotFoundModel model)
        {
            return model;
        }

        public MethodNotFoundModel GetNoHead()
        {
            return new MethodNotFoundModel
            {
                RequestMethod = requestContextResolver.GetContext().Request.Method
            };
        }

        public MethodNotFoundModel PutNoHead(MethodNotFoundModel model)
        {
            return model;
        }
    }

    public class MethodNotFoundModel
    {
        public string RequestMethod { get; set; }
    }
}
