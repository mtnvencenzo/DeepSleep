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
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAuthenticationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAuthenticationPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

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
                if (!(context.Configuration?.AllowAnonymous ?? false))
                {
                    var supportedAuthSchemes = context.Configuration.SupportedAuthenticationSchemes?.Count > 0
                        ? context.Configuration.SupportedAuthenticationSchemes.Where(a => a != null).Distinct().ToArray()
                        : new string[] { };

                    var providers = context.RequestServices
                        .GetServices<IAuthenticationProvider>()
                        .Where(p => supportedAuthSchemes.Length == 0 || supportedAuthSchemes.Contains(p.Scheme))
                        .ToList();

                    var authProvider = providers
                        .FirstOrDefault(p => p.CanHandleAuthScheme(context.Request.ClientAuthenticationInfo?.AuthScheme));

                    if (authProvider != null)
                    {
                        if (context.Request.ClientAuthenticationInfo == null)
                        {
                            context.Request.ClientAuthenticationInfo = new ClientAuthentication
                            {
                                AuthenticatedBy = AuthenticationType.Provider
                            };
                        }
                        else
                        {
                            context.Request.ClientAuthenticationInfo.AuthenticatedBy = AuthenticationType.Provider;
                        }

                        context.Request.ClientAuthenticationInfo.AuthResult = await authProvider.Authenticate(context).ConfigureAwait(false);
                    }
                    else
                    {
                        if (context.Request.ClientAuthenticationInfo == null)
                        {
                            context.Request.ClientAuthenticationInfo = new ClientAuthentication
                            {
                                AuthenticatedBy = AuthenticationType.None
                            };
                        }
                        else
                        {
                            context.Request.ClientAuthenticationInfo.AuthenticatedBy = AuthenticationType.None;
                        }
                    }

                    context.Request.ClientAuthenticationInfo.AuthResult = context.Request.ClientAuthenticationInfo.AuthResult ?? new AuthenticationResult(false);
                    context.Request.ClientAuthenticationInfo.Principal = context.Request.ClientAuthenticationInfo.AuthResult.Principal;

                    var result = context.Request.ClientAuthenticationInfo.AuthResult;
                    if (!result?.IsAuthenticated ?? false)
                    {
                        var challenges = new List<string>();
                        foreach (var p in providers)
                        {
                            if (!challenges.Contains($"{p.Scheme} realm=\"{p.Realm}\""))
                            {
                                challenges.Add($"{p.Scheme} realm=\"{p.Realm}\"");
                            }
                        };

                        context.Response.StatusCode = 401;
                        challenges.ForEach(c =>
                        {
                            context.Response.AddHeader(
                                name: "WWW-Authenticate", 
                                value: c, 
                                append: false, 
                                allowMultiple: true);
                        });

                        return false;
                    }
                }
                else
                {
                    if (context.Request.ClientAuthenticationInfo == null)
                    {
                        context.Request.ClientAuthenticationInfo = new ClientAuthentication
                        {
                            AuthenticatedBy = AuthenticationType.Anonymous,
                            AuthResult = new AuthenticationResult(true)
                        };
                    }
                    else
                    {
                        context.Request.ClientAuthenticationInfo.AuthenticatedBy = AuthenticationType.Anonymous;
                        context.Request.ClientAuthenticationInfo.AuthResult = new AuthenticationResult(true);
                    }
                }

                return true;
            }

            return false;
        }
    }
}
