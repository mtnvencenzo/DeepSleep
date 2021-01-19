namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;

    /// <summary>
    /// 
    /// </summary>
    public class EnableHeadController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        /// <summary>Initializes a new instance of the <see cref="EnableHeadController"/> class.</summary>
        /// <param name="requestContextResolver">The request context resolver.</param>
        public EnableHeadController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        /// <summary>Gets the with default enable head.</summary>
        /// <returns></returns>
        public RequestHeadModel GetWithDefaultEnableHead()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        /// <summary>Gets the with configured head enabled.</summary>
        /// <returns></returns>
        public RequestHeadModel GetWithConfiguredHeadEnabled()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        /// <summary>Gets the with configured head disbabled.</summary>
        /// <returns></returns>
        public RequestHeadModel GetWithConfiguredHeadDisbabled()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        /// <summary>Explicits the head.</summary>
        /// <returns></returns>
        public RequestHeadModel ExplicitHead()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }

        /// <summary>Gets the with explicit head.</summary>
        /// <returns></returns>
        public RequestHeadModel GetWithExplicitHead()
        {
            return new RequestHeadModel
            {
                Configured = requestContextResolver.GetContext().Configuration.EnableHeadForGetRequests
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RequestHeadModel
    {
        /// <summary>Gets or sets the configured.</summary>
        /// <value>The configured.</value>
        public bool? Configured { get; set; }
    }
}
