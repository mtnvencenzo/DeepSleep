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
        /// <returns></returns>
        public async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestAuthentication().ConfigureAwait(false))
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
        /// <returns></returns>
        /// <exception cref="Exception">No auth factory established for authenticated route
        /// or
        /// No auth providers established for authenticated route</exception>
        internal static async Task<bool> ProcessHttpRequestAuthentication(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!(context.RequestConfig?.AllowAnonymous ?? false))
                {
                    var providers = context.RequestServices
                        .GetServices<IAuthenticationProvider>()
                        .ToList();

                    var supportedAuthSchemes = context.RequestConfig.SupportedAuthenticationSchemes?.Count > 0
                        ? context.RequestConfig.SupportedAuthenticationSchemes.Where(a => a != null).Distinct().ToArray()
                        : new string[] { };

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

                        await authProvider.Authenticate(context).ConfigureAwait(false);
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
                    }

                    var result = context.RequestInfo.ClientAuthenticationInfo.AuthResult;

                    if (result == null || !result.IsAuthenticated)
                    {

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
                }

                return true;
            }

            return false;
        }
    }
}
