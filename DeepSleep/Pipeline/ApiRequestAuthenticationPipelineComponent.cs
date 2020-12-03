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
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver, IApiResponseMessageConverter responseMessageConverter)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestAuthentication(responseMessageConverter).ConfigureAwait(false))
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
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        /// <exception cref="Exception">No auth factory established for authenticated route
        /// or
        /// No auth providers established for authenticated route</exception>
        internal static async Task<bool> ProcessHttpRequestAuthentication(this ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!(context.RequestConfig?.AllowAnonymous ?? false))
                {
                    //logger?.LogInformation($"Using authentication scheme: ${{context.RequestInfo?.ClientAuthenticationInfo?.AuthScheme}}");

                    //logger?.LogDebug($"Endpoint does not allow anonymous access, preparing to authenticate request.");

                    var providers = context.RequestServices
                        .GetServices<IAuthenticationProvider>()
                        .ToList();

                    //logger?.LogDebug($"Found {providers.Count} authentication providers: {string.Join(", ", providers.Select(p => p.Scheme))}");

                    var supportedAuthSchemes = context.RequestConfig.SupportedAuthenticationSchemes?.Count > 0
                        ? context.RequestConfig.SupportedAuthenticationSchemes.Where(a => a != null).Distinct().ToArray()
                        : new string[] { };

                    if (supportedAuthSchemes.Length > 0)
                    {
                        //logger?.LogDebug($"Endpoint is configured using these supported auth schemes: {string.Join(", ", supportedAuthSchemes)}");
                    }

                    var authProvider = providers
                        .Where(p => supportedAuthSchemes.Length == 0 || supportedAuthSchemes.Contains(p.Scheme))
                        .FirstOrDefault(p => p.CanHandleAuthScheme(context.RequestInfo.ClientAuthenticationInfo?.AuthScheme));

                    if (authProvider != null)
                    {
                        if (context.RequestInfo.ClientAuthenticationInfo == null)
                        {
                            context.RequestInfo.ClientAuthenticationInfo = new ClientAuthentication
                            {
                                AuthenticatedBy = AuthenticationType.Provider
                            };
                        }
                        else
                        {
                            context.RequestInfo.ClientAuthenticationInfo.AuthenticatedBy = AuthenticationType.Provider;
                        }

                        //logger?.LogInformation($"Authentication provider using scheme {authProvider.Scheme} was match and will authenticate the request.");

                        await authProvider.Authenticate(context, responseMessageConverter).ConfigureAwait(false);
                    }
                    else
                    {
                        if (context.RequestInfo.ClientAuthenticationInfo == null)
                        {
                            context.RequestInfo.ClientAuthenticationInfo = new ClientAuthentication
                            {
                                AuthenticatedBy = AuthenticationType.None
                            };
                        }
                        else
                        {
                            context.RequestInfo.ClientAuthenticationInfo.AuthenticatedBy = AuthenticationType.None;
                        }
                        //logger?.LogWarning($"No Authentication provider was found for client request scheme {context.RequestInfo?.ClientAuthenticationInfo?.AuthScheme}.");
                    }

                    var result = context.RequestInfo.ClientAuthenticationInfo.AuthResult;

                    if (result == null || !result.IsAuthenticated)
                    {
                        if (result == null)
                        {
                            //logger?.LogWarning($"Request failed authentication,  auth result is null");
                        }
                        else
                        {
                            //logger?.LogWarning($"Request failed authentication with errors {string.Join(", ", result.Errors ?? new List<ApiResponseMessage>())}");
                        }

                        if (providers.FirstOrDefault() == null)
                        {
                            throw new Exception("No authentication providers established for authenticated route");
                        }

                        var challenges = new List<string>();
                        foreach (var p in providers)
                        {
                            if (!challenges.Contains($"{p.Scheme} realm=\"{p.Realm}\""))
                            {
                                challenges.Add($"{p.Scheme} realm=\"{p.Realm}\"");
                            }
                        };

                        challenges.ForEach(c => context.ResponseInfo.AddHeader("WWW-Authenticate", c));

                        context.ResponseInfo.StatusCode = 401;

                        return false;
                    }
                }
                else
                {
                    if (context.RequestInfo.ClientAuthenticationInfo == null)
                    {
                        context.RequestInfo.ClientAuthenticationInfo = new ClientAuthentication
                        {
                            AuthenticatedBy = AuthenticationType.Anonymous,
                            AuthResult = new AuthenticationResult(true)
                        };
                    }
                    else
                    {
                        context.RequestInfo.ClientAuthenticationInfo.AuthenticatedBy = AuthenticationType.Anonymous;
                        context.RequestInfo.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(true);
                    }
                    //logger?.LogDebug($"Client request is for anonymous endpoint, skipping authentication");
                }

                //logger?.LogInformation($"Client request was successfully authenticated using scheme: {context.RequestInfo?.ClientAuthenticationInfo?.AuthScheme}.");

                return true;
            }

            return false;
        }
    }
}
