using DeepSleep.Formatting;
using DeepSleep.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeepSleep.Validation;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiServiceConfiguration
    {
        /// <summary>Gets or sets the routing table resolver.</summary>
        /// <value>The routing table resolver.</value>
        Func<IServiceProvider, IApiRoutingTable, IApiRoutingTable> RoutingTableResolver { get; set; }

        /// <summary>Gets or sets the authentication factory resolver.</summary>
        /// <value>The authentication factory resolver.</value>
        Func<IServiceProvider, IAuthenticationFactory, IAuthenticationFactory> AuthenticationFactoryResolver { get; set; }

        /// <summary>Gets or sets the format factory resover.</summary>
        /// <value>The format factory resover.</value>
        Func<IServiceProvider, IFormatStreamReaderWriterFactory, IFormatStreamReaderWriterFactory> FormatFactoryResolver { get; set; }

        /// <summary>Gets or sets the cross origin expose headers.</summary>
        /// <value>The cross origin expose headers.</value>
        Func<IServiceProvider, ICrossOriginConfigResolver, ICrossOriginConfigResolver> CrossOriginConfigResolver { get; set; }

        /// <summary>Gets or sets the API request pipeline builder.</summary>
        /// <value>The API request pipeline builder.</value>
        Func<IServiceProvider, IApiRequestPipeline, IApiRequestPipeline> ApiRequestPipelineBuilder { get; set; }

        /// <summary>Gets or sets the API validation provider.</summary>
        /// <value>The API validation provider.</value>
        Func<IServiceProvider, IApiValidationProvider, IApiValidationProvider> ApiValidationProvider { get; set; }

        /// <summary>Gets or sets the API response message processor provider.</summary>
        /// <value>The API response message processor provider.</value>
        Func<IServiceProvider, IApiResponseMessageProcessorProvider, IApiResponseMessageProcessorProvider> ApiResponseMessageProcessorProvider { get; set; }

        /// <summary>Gets or sets the API response message converter.</summary>
        /// <value>The API response message converter.</value>
        Func<IServiceProvider, IApiResponseMessageConverter, IApiResponseMessageConverter> ApiResponseMessageConverter { get; set; }

        /// <summary>Gets or sets the exception handler.</summary>
        /// <value>The exception handler.</value>
        Func<ApiRequestContext, Exception, Task<long>> ExceptionHandler { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool UsePingEndpoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool UseEnvironmentEndpoint { get; set; }

        /// <summary>Adds the pipeline hook.</summary>
        /// <param name="types">The types.</param>
        /// <param name="placements">The placements.</param>
        /// <param name="hook">The hook.</param>
        /// <returns></returns>
        IApiServiceConfiguration AddPipelineHook(ApiRequestPipelineComponentTypes types, ApiRequestPipelineHookPlacements placements, Func<ApiRequestContext, ApiRequestPipelineComponentTypes, ApiRequestPipelineHookPlacements, Task<ApiRequestPipelineHookResult>> hook);

        /// <summary>Gets the pipeline hooks.</summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        IEnumerable<ApiRequestPipelineHookContainer> GetPipelineHooks(ApiRequestPipelineComponentTypes types);
    }
}
