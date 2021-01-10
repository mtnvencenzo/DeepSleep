namespace DeepSleep.Tests
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

        /// <summary>Gets or set the json formatting configuration
        /// </summary>
        public IJsonFormattingConfiguration JsonConfiguration { get; set; }

        /// <summary>Gets or sets a list of regular expression paths to exclude from processing</summary>
        /// <value>The paths to exclude.</value>
        public IList<string> ExcludePaths { get; set; }

        /// <summary>Gets or sets a value indicating whether [write console header].</summary>
        /// <value><c>true</c> if [write console header]; otherwise, <c>false</c>.</value>
        public bool WriteConsoleHeader { get; set; } = false;

        /// <summary>Gets the default request pipeline.</summary>
        /// <returns></returns>
        internal static IApiRequestPipeline GetDefaultRequestPipeline()
        {
            return new ApiRequestPipeline()
                .UseApiResponseUnhandledExceptionHandler()
                .UseApiRequestCanceled()
                .UseApiResponseBodyWriter()
                .UseApiResponseCookies()
                .UseApiResponseMessages()
                .UseApiResponseHttpCaching()
                .UseApiRequestUriValidation()
                .UseApiRequestHeaderValidation()
                .UseApiResponseCorrelation()
                .UseApiResponseDeprecated()
                .UseApiRequestRouting()
                .UseApiRequestLocalization()
                .UseApiRequestNotFound()
                .UseApiResponseCors()
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
