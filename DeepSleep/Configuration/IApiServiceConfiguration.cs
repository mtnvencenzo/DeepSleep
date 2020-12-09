namespace DeepSleep.Configuration
{
    using DeepSleep.Formatting;
    using System;
    using System.Threading.Tasks;
    using DeepSleep.Validation;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiServiceConfiguration
    {
        /// <summary>Gets or sets the routing table.</summary>
        /// <value>The routing table.</value>
        IApiRoutingTable RoutingTable { get; set; }

        /// <summary>Gets or sets the format factory.</summary>
        /// <value>The format factory.</value>
        IFormatStreamReaderWriterFactory FormatterFactory { get; set; }

        /// <summary>Gets or sets the API request pipeline.</summary>
        /// <value>The API request pipeline.</value>
        IApiRequestPipeline ApiRequestPipeline { get; set; }

        /// <summary>Gets or sets the API validation provider.</summary>
        /// <value>The API validation provider.</value>
        IApiValidationProvider ApiValidationProvider { get; set; }

        /// <summary>Gets or sets the default request configuration.</summary>
        /// <value>The default request configuration.</value>
        IApiRequestConfiguration DefaultRequestConfiguration { get; set; }

        /// <summary>Gets or sets the exception handler.</summary>
        /// <value>The exception handler.</value>
        Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler { get; set; }

        /// <summary>Gets or sets the default json formatting configuration
        /// </summary>
        IJsonFormattingConfiguration JsonConfiguration { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IList<string> ExcludePaths { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool UsePingEndpoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool UseEnvironmentEndpoint { get; set; }
    }
}
