namespace DeepSleep.Pipeline
{
    using DeepSleep.Auth;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestAuthenticationPipelineComponent : PipelineComponentBase
    {
        private readonly ApiRequestDelegate apinext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAuthenticationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAuthenticationPipelineComponent(ApiRequestDelegate next)
        {
            apinext = next;
        }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="authFactory">The authentication factory.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IAuthenticationFactory authFactory, IApiResponseMessageConverter responseMessageConverter)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestAuthentication(authFactory, responseMessageConverter).ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestAuthenticationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request authentication.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestAuthentication(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestAuthenticationPipelineComponent>();
        }

        /// <summary>Processes the HTTP request authentication.</summary>
        /// <param name="context">The context.</param>
        /// <param name="authFactory">The authentication factory.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        /// <exception cref="Exception">No auth factory established for authenticated route
        /// or
        /// No auth providers established for authenticated route</exception>
        public static async Task<bool> ProcessHttpRequestAuthentication(this ApiRequestContext context, IAuthenticationFactory authFactory, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!(context.RequestConfig?.AllowAnonymous ?? false))
                {
                    if (authFactory == null)
                    {
                        throw new Exception("No auth factory established for authenticated route");
                    }

                    var authProvider = authFactory?.GetProvider(context.RequestInfo?.ClientAuthenticationInfo?.AuthScheme);
                    var result = (authProvider != null)
                        ? await authProvider.Authenticate(context, responseMessageConverter).ConfigureAwait(false)
                        : null;

                    if (result == null || !result.IsAuthenticated)
                    {
                        if (authFactory.FirstOrDefault() == null)
                        {
                            throw new Exception("No auth providers established for authenticated route");
                        }

                        var challenges = new List<string>();
                        foreach (var p in authFactory.GetProviders())
                        {
                            if (!challenges.Contains($"{p.AuthScheme} realm=\"{p.Realm}\""))
                            {
                                challenges.Add($"{p.AuthScheme} realm=\"{p.Realm}\"");
                            }
                        };


                        challenges.ForEach(c => context.ResponseInfo.AddHeader("WWW-Authenticate", c));
                        context.ResponseInfo.ResponseObject = new ApiResponse
                        {
                            StatusCode = 401
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
