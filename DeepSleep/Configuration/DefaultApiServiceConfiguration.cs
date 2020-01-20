namespace DeepSleep.Configuration
{
    using DeepSleep.Formatting;
    using DeepSleep.Auth;
    using System;
    using System.Threading.Tasks;
    using DeepSleep.Validation;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultApiServiceConfiguration : IApiServiceConfiguration
    {
        /// <summary>Gets or sets the routing table.</summary>
        /// <value>The routing table.</value>
        public IApiRoutingTable RoutingTable { get; set; }

        /// <summary>Gets or sets the authentication factory.</summary>
        /// <value>The authentication factory.</value>
        public IAuthenticationFactory AuthenticationFactory { get; set; }

        /// <summary>Gets or sets the format factory.</summary>
        /// <value>The format factory.</value>
        public IFormatStreamReaderWriterFactory FormatterFactory { get; set; }

        /// <summary>Gets or sets the API request pipeline.</summary>
        /// <value>The API request pipeline.</value>
        public IApiRequestPipeline ApiRequestPipeline { get; set; }

        /// <summary>Gets or sets the API validation provider.</summary>
        /// <value>The API validation provider.</value>
        public IApiValidationProvider ApiValidationProvider { get; set; }

        /// <summary>Gets or sets the API response message processor provider.</summary>
        /// <value>The API response message processor provider.</value>
        public IApiResponseMessageProcessorProvider ApiResponseMessageProcessorProvider { get; set; }

        /// <summary>Gets or sets the API response message converter.</summary>
        /// <value>The API response message converter.</value>
        public IApiResponseMessageConverter ApiResponseMessageConverter { get; set; }

        /// <summary>Gets or sets the default request configuration.</summary>
        /// <value>The default request configuration.</value>
        public IApiRequestConfiguration DefaultRequestConfiguration { get; set; }

        /// <summary>Gets or sets the exception handler.</summary>
        /// <value>The exception handler.</value>
        public virtual Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler { get; set; }

        /// <summary>
        /// </summary>
        public bool UsePingEndpoint { get; set; } = true;

        /// <summary>
        /// </summary>
        public bool UseEnvironmentEndpoint { get; set; } = true;
    }
}
