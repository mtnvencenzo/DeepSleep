﻿namespace DeepSleep.Pipeline
{
    using DeepSleep.Auth;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestAuthorizationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAuthorizationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAuthorizationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }


        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="responseMessageConverter">The responseMessageConverter.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiResponseMessageConverter responseMessageConverter)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestAuthorization(responseMessageConverter).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestAuthorizationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request authorization.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestAuthorization(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestAuthorizationPipelineComponent>();
        }

        /// <summary>Processes the HTTP request authorization.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageConverter">The responseMessageConverter.</param>
        /// <returns></returns>
        public static async Task<bool> ProcessHttpRequestAuthorization(this ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!(context.RequestConfig?.AllowAnonymous ?? false) && !string.IsNullOrWhiteSpace(context.RequestInfo?.ClientAuthorizationInfo?.Policy))
                {
                    var providers = context.RequestServices
                        .GetServices<IAuthorizationProvider>()
                        .ToList();

                    IAuthorizationProvider authProvider = null;

                    try
                    {
                        authProvider = providers.FirstOrDefault(p => p.CanHandleAuthPolicy(context.RequestInfo?.ClientAuthorizationInfo?.Policy));
                    }
                    catch (System.Exception)
                    {
                        // TODO: log message
                    }

                    if (authProvider != null)
                    {
                        await authProvider.Authorize(context, responseMessageConverter).ConfigureAwait(false);
                    }

                    var result = context.RequestInfo?.ClientAuthorizationInfo?.AuthResult;

                    if (result == null || !result.IsAuthorized)
                    {
                        if (authProvider == null)
                        {
                            throw new Exception($"No authorization providers established for authenticated route using policy '{context.RequestInfo?.ClientAuthorizationInfo?.Policy}'");
                        }

                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 403
                        };
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
