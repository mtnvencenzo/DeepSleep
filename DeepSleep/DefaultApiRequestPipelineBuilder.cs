﻿using DeepSleep.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiRequestPipelineBuilder" />
    public class DefaultApiRequestPipelineBuilder : IApiRequestPipelineBuilder
    {
        /// <summary>Builds this instance.</summary>
        /// <returns></returns>
        public IApiRequestPipeline Build()
        {
            return new ApiRequestPipeline()
                .UseApiResponseUnhandledExceptionHandler()
                .UseApiRequestCanceled()
                .UseApiHttpComformance()
                .UseApiResponseBodyWriter()
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
