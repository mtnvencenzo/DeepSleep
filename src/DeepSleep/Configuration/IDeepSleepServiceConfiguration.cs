namespace DeepSleep.Configuration
{
    using DeepSleep.Discovery;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepServiceConfiguration
    {
        /// <summary>Gets or sets the discovery strategies.</summary>
        /// <value>The discovery strategies.</value>
        IList<IDeepSleepDiscoveryStrategy> DiscoveryStrategies { get; set; }

        /// <summary>Gets or sets the default request configuration.</summary>
        /// <value>The default request configuration.</value>
        IDeepSleepRequestConfiguration DefaultRequestConfiguration { get; set; }

        /// <summary>Gets or sets the on exception.</summary>
        /// <value>The on exception.</value>
        Func<IApiRequestContextResolver, Exception, Task> OnException { get; set; }

        /// <summary>Gets or sets the on request processed.</summary>
        /// <value>The on request processed.</value>
        Func<IApiRequestContextResolver, Task> OnRequestProcessed { get; set; }

        /// <summary>Gets or sets the exclude paths.</summary>
        /// <value>The exclude paths.</value>
        IList<string> ExcludePaths { get; set; }

        /// <summary>Gets or sets the route prefix.</summary>
        /// <value>The route prefix.</value>
        string RoutePrefix { get; set; }

        /// <summary>Gets or sets a value indicating whether [write console header].</summary>
        /// <value><c>true</c> if [write console header]; otherwise, <c>false</c>.</value>
        bool WriteConsoleHeader { get; set; }
    }
}
