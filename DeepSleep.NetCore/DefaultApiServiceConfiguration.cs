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

        /// <summary>Gets or sets the API validation provider.</summary>
        /// <value>The API validation provider.</value>
        public IApiValidationProvider ApiValidationProvider { get; set; }

        /// <summary>Gets or sets the default request configuration.</summary>
        /// <value>The default request configuration.</value>
        public IApiRequestConfiguration DefaultRequestConfiguration { get; set; }

        /// <summary>Gets or sets the exception handler.</summary>
        /// <value>The exception handler.</value>
        public virtual Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler { get; set; }

        /// <summary>Gets or set the json formatting configuration
        /// </summary>
        public IJsonFormattingConfiguration JsonConfiguration { get; set; }

        /// <summary>Gets or sets a list of regular expression paths to exclude from processing</summary>
        /// <value>The paths to exclude.</value>
        public IList<string> ExcludePaths { get; set; }

        /// <summary>Gets or sets the ping endpoint.</summary>
        /// <value>The ping endpoint.</value>
        public EndpointUsage PingEndpoint { get; set; } = new EndpointUsage { Enabled = true, RelativePath = "ping" };

        /// <summary>Gets or sets a value indicating whether [write console header].</summary>
        /// <value><c>true</c> if [write console header]; otherwise, <c>false</c>.</value>
        public bool WriteConsoleHeader { get; set; } = true;

        /// <summary>Gets the default request pipeline.</summary>
        /// <returns></returns>
        internal static IApiRequestPipeline GetDefaultRequestPipeline()
        {
            return new ApiRequestPipeline()
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
