namespace DeepSleep.NetCore
{
    using DeepSleep.Configuration;
    using DeepSleep.Discovery;
    using DeepSleep.Formatting;
    using DeepSleep.Pipeline;
    using DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Configuration.IApiServiceConfiguration" />
    public class DefaultApiServiceConfiguration : IApiServiceConfiguration
    {
        /// <summary>Gets or sets the routing table.</summary>
        /// <value>The routing table.</value>
        public IList<IRouteDiscoveryStrategy> DiscoveryStrategies { get; set; }

        /// <summary>Gets or sets the default request configuration.</summary>
        /// <value>The default request configuration.</value>
        public IApiRequestConfiguration DefaultRequestConfiguration { get; set; }

        /// <summary>Gets or sets the on exception.</summary>
        /// <value>The on exception.</value>
        public virtual Func<ApiRequestContext, Exception, Task> OnException { get; set; }

        /// <summary>Gets or sets the on request processed.</summary>
        /// <value>The on request processed.</value>
        public virtual Func<ApiRequestContext, Task> OnRequestProcessed { get; set; }

        /// <summary>Gets or sets the json formatter configuration.</summary>
        /// <value>The json formatter configuration.</value>
        public JsonFormattingConfiguration JsonFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the XML formatter configuration.</summary>
        /// <value>The XML formatter configuration.</value>
        public XmlFormattingConfiguration XmlFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the multipart form data formatter configuration.</summary>
        /// <value>The multipart form data formatter configuration.</value>
        public MultipartFormDataFormattingConfiguration MultipartFormDataFormatterConfiguration { get; set; }

        /// <summary>Gets or sets the form URL encoded formatter configuration.</summary>
        /// <value>The form URL encoded formatter configuration.</value>
        public FormUrlEncodedFormattingConfiguration FormUrlEncodedFormatterConfiguration { get; set; }

        /// <summary>Gets or sets a list of regular expression paths to exclude from processing</summary>
        /// <value>The paths to exclude.</value>
        public IList<string> ExcludePaths { get; set; }

        /// <summary>Gets or sets the ping endpoint.</summary>
        /// <value>The ping endpoint.</value>
        public EndpointUsage PingEndpoint { get; set; } = new EndpointUsage { Enabled = true, RelativePath = "ping" };

        /// <summary>Gets or sets a value indicating whether [write console header].</summary>
        /// <value><c>true</c> if [write console header]; otherwise, <c>false</c>.</value>
        public bool WriteConsoleHeader { get; set; } = true;
    }
}
