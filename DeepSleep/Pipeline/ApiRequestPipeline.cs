namespace DeepSleep.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestPipeline : IApiRequestPipeline
    {
        private readonly IList<ApiRequestDelegateHandler> pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestPipeline"/> class.
        /// </summary>
        public ApiRequestPipeline()
        {
            this.pipeline = new List<ApiRequestDelegateHandler>();
        }

        /// <summary>Uses the pipeline component.</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IApiRequestPipeline UsePipelineComponent<T>() where T : IPipelineComponent
        {
            var delegateHandler = new ApiRequestDelegateHandler(this, typeof(T));
            pipeline.Add(delegateHandler);

            return this;
        }

        /// <summary>Runs this instance.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public virtual async Task Run(IApiRequestContextResolver contextResolver)
        {
            var first = pipeline.FirstOrDefault();

            if (first != null)
            {
                await first.TaskInvoker(contextResolver).ConfigureAwait(false);
            }
        }

        /// <summary>Gets the registered pipeline.</summary>
        /// <value>The registered pipeline.</value>
        Dictionary<int, ApiRequestDelegateHandler> IApiRequestPipeline.RegisteredPipeline => pipeline.ToDictionary(p => this.pipeline.IndexOf(p), p => p);

        /// <summary>Gets the default request pipeline.</summary>
        /// <returns></returns>
        public static IApiRequestPipeline GetDefaultRequestPipeline()
        {
            return new ApiRequestPipeline()
                .UseApiResponseRequestProcessing()
                .UseApiResponseUnhandledExceptionHandler()
                .UseApiResponseRequesId()
                .UseApiRequestCanceled()
                .UseApiResponseBodyWriter()
                .UseApiResponseCookies()
                .UseApiResponseMessages()
                .UseApiResponseHttpCaching()
                .UseApiResponseCorrelation()
                .UseApiResponseDeprecated()
                .UseApiResponseCors()
                .UseApiRequestRouting()
                .UseApiRequestUriValidation()
                .UseApiRequestHeaderValidation()
                .UseApiRequestLocalization()
                .UseApiRequestNotFound()
                .UseApiRequestCorsPreflight()
                .UseApiRequestMethod()
                .UseApiRequestAccept()
                .UseApiRequestAuthentication()
                .UseApiRequestAuthorization()
                .UseApiRequestInvocationInitializer()
                .UseApiRequestUriBinding()
                .UseApiRequestBodyBinding()
                .UseApiRequestEndpointValidation()
                .UseApiRequestEndpointInvocation();
        }
    }
}
