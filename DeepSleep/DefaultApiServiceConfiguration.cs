namespace DeepSleep
{
    using DeepSleep.Formatting;
    using DeepSleep.Auth;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DeepSleep.Validation;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultApiServiceConfiguration : IApiServiceConfiguration
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="DefaultApiServiceConfiguration"/> class.</summary>
        public DefaultApiServiceConfiguration() : base()
        {
            _hooks = new List<ApiRequestPipelineHookContainer>();

            RoutingTableResolver = (s, p) => p;
            FormatFactoryResolver = (s, p) => p;
            AuthenticationFactoryResolver = (s, p) => p;
            CrossOriginConfigResolver = (s, p) => p;
            ApiRequestPipelineBuilder = (s, p) => p;
            ApiValidationProvider = (s, p) => p;
            ApiResponseMessageProcessorProvider = (s, p) => p;
            ApiResponseMessageConverter = (s, p) => p;
        }

        private List<ApiRequestPipelineHookContainer> _hooks;

        #endregion

        /// <summary>Gets or sets the routing table resolver.</summary>
        /// <value>The routing table resolver.</value>
        public virtual Func<IServiceProvider, IApiRoutingTable, IApiRoutingTable> RoutingTableResolver { get; set; }

        /// <summary>Gets or sets the authentication factory resolver.</summary>
        /// <value>The authentication factory resolver.</value>
        public virtual Func<IServiceProvider, IAuthenticationFactory, IAuthenticationFactory> AuthenticationFactoryResolver { get; set; }

        /// <summary>Gets or sets the format factory resover.</summary>
        /// <value>The format factory resover.</value>
        public virtual Func<IServiceProvider, IFormatStreamReaderWriterFactory, IFormatStreamReaderWriterFactory> FormatFactoryResolver { get; set; }

        /// <summary>Gets or sets the cross origin expose headers.</summary>
        /// <value>The cross origin expose headers.</value>
        public virtual Func<IServiceProvider, ICrossOriginConfigResolver, ICrossOriginConfigResolver> CrossOriginConfigResolver { get; set; }

        /// <summary>Gets or sets the API request pipeline builder.</summary>
        /// <value>The API request pipeline builder.</value>
        public virtual Func<IServiceProvider, IApiRequestPipeline, IApiRequestPipeline> ApiRequestPipelineBuilder { get; set; }

        /// <summary>Gets or sets the API validation provider.</summary>
        /// <value>The API validation provider.</value>
        public virtual Func<IServiceProvider, IApiValidationProvider, IApiValidationProvider> ApiValidationProvider { get; set; }

        /// <summary>Gets or sets the API response message processor provider.</summary>
        /// <value>The API response message processor provider.</value>
        public virtual Func<IServiceProvider, IApiResponseMessageProcessorProvider, IApiResponseMessageProcessorProvider> ApiResponseMessageProcessorProvider { get; set; }

        /// <summary>Gets or sets the API response message converter.</summary>
        /// <value>The API response message converter.</value>
        public virtual Func<IServiceProvider, IApiResponseMessageConverter, IApiResponseMessageConverter> ApiResponseMessageConverter { get; set; }

        /// <summary>Gets or sets the exception handler.</summary>
        /// <value>The exception handler.</value>
        public virtual Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool UsePingEndpoint { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool UseEnvironmentEndpoint { get; set; } = true;

        /// <summary>Adds the pipeline hook.</summary>
        /// <param name="types">The types.</param>
        /// <param name="placements">The placements.</param>
        /// <param name="hook">The hook.</param>
        /// <returns></returns>
        public virtual IApiServiceConfiguration AddPipelineHook(ApiRequestPipelineComponentTypes types, ApiRequestPipelineHookPlacements placements, Func<ApiRequestContext, ApiRequestPipelineComponentTypes, ApiRequestPipelineHookPlacements, Task<ApiRequestPipelineHookResult>> hook)
        {
            _hooks.Add(new ApiRequestPipelineHookContainer
            {
                Hook = hook,
                PipelineTypes = types,
                Placements = placements
            });

            return this;
        }

        /// <summary>Gets the pipeline hooks.</summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        public virtual IEnumerable<ApiRequestPipelineHookContainer> GetPipelineHooks(ApiRequestPipelineComponentTypes types)
        {
            foreach (var hook in _hooks)
            {
                if (hook.PipelineTypes.HasFlag(types))
                    yield return hook;
            }
        }
    }
}
