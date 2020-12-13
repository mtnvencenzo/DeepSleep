namespace DeepSleep.Tests
{
    using DeepSleep.Formatting;
    using System;
    using System.Threading.Tasks;
    using DeepSleep.Validation;
    using System.Collections.Generic;
    using DeepSleep.Pipeline;
    using DeepSleep.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class DefaultApiServiceConfiguration : IApiServiceConfiguration
    {
        /// <summary>Gets or sets the routing table.</summary>
        /// <value>The routing table.</value>
        public IApiRoutingTable RoutingTable { get; set; }

        /// <summary>Gets or sets the format factory.</summary>
        /// <value>The format factory.</value>
        public IFormatStreamReaderWriterFactory FormatterFactory { get; set; }

        /// <summary>Gets or sets the API request pipeline.</summary>
        /// <value>The API request pipeline.</value>
        public IApiRequestPipeline ApiRequestPipeline { get; set; }

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

        /// <summary>Gets the default request pipeline.</summary>
        /// <returns></returns>
        internal static IApiRequestPipeline GetDefaultRequestPipeline()
        {
            return new ApiRequestPipeline()
                .UseApiResponseUnhandledExceptionHandler()
                .UseApiRequestCanceled()
                .UseApiHttpComformance()
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
