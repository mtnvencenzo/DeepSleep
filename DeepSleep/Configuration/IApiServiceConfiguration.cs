namespace DeepSleep.Configuration
{
    using DeepSleep.Formatting;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiServiceConfiguration
    {
        /// <summary>Gets or sets the routing table.</summary>
        /// <value>The routing table.</value>
        IApiRoutingTable RoutingTable { get; set; }

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

        /// <summary>Gets or sets the exclude paths.</summary>
        /// <value>The exclude paths.</value>
        IList<string> ExcludePaths { get; set; }
    }
}
