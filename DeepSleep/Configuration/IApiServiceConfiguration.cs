namespace DeepSleep.Configuration
{
    using DeepSleep.Discovery;
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiServiceConfiguration
    {
        /// <summary>Gets or sets the discovery strategies.</summary>
        /// <value>The discovery strategies.</value>
        IList<IRouteDiscoveryStrategy> DiscoveryStrategies { get; set; }

        /// <summary>Gets or sets the default request configuration.</summary>
        /// <value>The default request configuration.</value>
        IApiRequestConfiguration DefaultRequestConfiguration { get; set; }

        /// <summary>Gets or sets the on exception.</summary>
        /// <value>The on exception.</value>
        Func<ApiRequestContext, Exception, Task> OnException { get; set; }

        /// <summary>Gets or sets the on request processed.</summary>
        /// <value>The on request processed.</value>
        Func<ApiRequestContext, Task> OnRequestProcessed { get; set; }

        /// <summary>Gets or sets the json formatter configuration.</summary>
        /// <value>The json formatter configuration.</value>
        JsonFormattingConfiguration JsonFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the XML formatter configuration.</summary>
        /// <value>The XML formatter configuration.</value>
        XmlFormattingConfiguration XmlFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the multipart form data formatter configuration.</summary>
        /// <value>The multipart form data formatter configuration.</value>
        MultipartFormDataFormattingConfiguration MultipartFormDataFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the form URL encoded formatter configuration.</summary>
        /// <value>The form URL encoded formatter configuration.</value>
        FormUrlEncodedFormattingConfiguration FormUrlEncodedFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the exclude paths.</summary>
        /// <value>The exclude paths.</value>
        IList<string> ExcludePaths { get; set; }

        /// <summary>Gets or sets a value indicating whether [write console header].</summary>
        /// <value><c>true</c> if [write console header]; otherwise, <c>false</c>.</value>
        bool WriteConsoleHeader { get; set; }
    }
}
