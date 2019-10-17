namespace DeepSleep
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum ApiRequestPipelineComponentTypes
    {
        /// <summary>The none</summary>
        None = 0,

        /// <summary>
        /// The HTTP conformance pipeline
        /// </summary>
        HttpConformancePipeline = 1,

        /// <summary>
        /// The request accept pipeline
        /// </summary>
        RequestAcceptPipeline = 2,

        /// <summary>
        /// The request authentication pipeline
        /// </summary>
        RequestAuthenticationPipeline = 4,

        /// <summary>
        /// The request authorization pipeline
        /// </summary>
        RequestAuthorizationPipeline = 8,

        /// <summary>
        /// The request body binding pipeline
        /// </summary>
        RequestBodyBindingPipeline = 16,

        /// <summary>
        /// The request canceled pipeline component
        /// </summary>
        RequestCanceledPipelineComponent = 32,

        /// <summary>
        /// The request cors preflight pipeline
        /// </summary>
        RequestCorsPreflightPipeline = 64,

        /// <summary>
        /// The request endpoint invocation pipeline
        /// </summary>
        RequestEndpointInvocationPipeline = 128,

        /// <summary>
        /// The request endpoint validation pipeline
        /// </summary>
        RequestEndpointValidationPipeline = 256,

        /// <summary>
        /// The request header validation pipeline
        /// </summary>
        RequestHeaderValidationPipeline = 512,

        /// <summary>
        /// The request invocation initializer pipeline
        /// </summary>
        RequestInvocationInitializerPipeline = 1024,

        /// <summary>
        /// The request localization pipeline
        /// </summary>
        RequestLocalizationPipeline = 2048,

        /// <summary>
        /// The request method pipeline
        /// </summary>
        RequestMethodPipeline = 4096,

        /// <summary>
        /// The request not found pipeline
        /// </summary>
        RequestNotFoundPipeline = 8192,

        /// <summary>
        /// The request routing pipeline
        /// </summary>
        RequestRoutingPipeline = 16384,

        /// <summary>
        /// The request URI binding pipeline
        /// </summary>
        RequestUriBindingPipeline = 32768,

        /// <summary>
        /// The request URI validation pipeline
        /// </summary>
        RequestUriValidationPipeline = 65536,

        /// <summary>
        /// The response body writer pipeline
        /// </summary>
        ResponseBodyWriterPipeline = 131072,

        /// <summary>
        /// The response correlation pipeline
        /// </summary>
        ResponseCorrelationPipeline = 262144,

        /// <summary>
        /// The response cors pipeline
        /// </summary>
        ResponseCorsPipeline = 524288,

        /// <summary>
        /// The response HTTP caching pipeline
        /// </summary>
        ResponseHttpCachingPipeline = 1048576,

        /// <summary>
        /// The response message pipeline
        /// </summary>
        ResponseMessagePipeline = 2097152,

        /// <summary>
        /// The response unhandled exception pipeline
        /// </summary>
        ResponseUnhandledExceptionPipeline = 4194304,

        /// <summary>The response deprecated pipeline</summary>
        ResponseDeprecatedPipeline = 8388608,
    }
}
