namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;

    /// <summary>
    /// 
    /// </summary>
    public class MethodNotFoundController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        /// <summary>Initializes a new instance of the <see cref="MethodNotFoundController"/> class.</summary>
        /// <param name="requestContextResolver">The request context resolver.</param>
        public MethodNotFoundController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        /// <summary>Gets this instance.</summary>
        /// <returns></returns>
        public MethodNotFoundModel Get()
        {
            return new MethodNotFoundModel
            {
                RequestMethod = requestContextResolver.GetContext().Request.Method
            };
        }

        /// <summary>Puts the specified model.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MethodNotFoundModel Put(MethodNotFoundModel model)
        {
            return model;
        }

        /// <summary>Gets the no head.</summary>
        /// <returns></returns>
        public MethodNotFoundModel GetNoHead()
        {
            return new MethodNotFoundModel
            {
                RequestMethod = requestContextResolver.GetContext().Request.Method
            };
        }

        /// <summary>Puts the no head.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public MethodNotFoundModel PutNoHead(MethodNotFoundModel model)
        {
            return model;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MethodNotFoundModel
    {
        /// <summary>Gets or sets the request method.</summary>
        /// <value>The request method.</value>
        public string RequestMethod { get; set; }
    }
}
