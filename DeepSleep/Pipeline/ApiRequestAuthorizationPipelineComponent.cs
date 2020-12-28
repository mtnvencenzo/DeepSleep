namespace DeepSleep.Pipeline
{
    using DeepSleep.Auth;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestAuthorizationPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestAuthorizationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestAuthorizationPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver.GetContext();

            if (await context.ProcessHttpRequestAuthorization().ConfigureAwait(false))
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
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpRequestAuthorization(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                if (!(context.Configuration?.AllowAnonymous ?? false))
                {
                    if (!string.IsNullOrWhiteSpace(context.Configuration?.AuthorizationConfig?.Policy))
                    {
                        var providers = context.RequestServices
                            .GetServices<IAuthorizationProvider>()
                            .ToList();

                        var authProvider = providers
                            .FirstOrDefault(p => p.CanHandleAuthPolicy(context.Configuration.AuthorizationConfig.Policy));

                        if (authProvider != null)
                        {
                            if (context.Request.ClientAuthorizationInfo == null)
                            {
                                context.Request.ClientAuthorizationInfo = new ClientAuthorization
                                {
                                    AuthorizedBy = AuthorizationType.Provider
                                };
                            }
                            else
                            {
                                context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.Provider;
                            }

                            context.Request.ClientAuthorizationInfo.AuthResult = await authProvider.Authorize(context).ConfigureAwait(false);
                        }
                        else
                        {
                            if (context.Request.ClientAuthorizationInfo == null)
                            {
                                context.Request.ClientAuthorizationInfo = new ClientAuthorization
                                {
                                    AuthorizedBy = AuthorizationType.None
                                };
                            }
                            else
                            {
                                context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.None;
                            }
                        }

                        context.Request.ClientAuthorizationInfo.AuthResult = context.Request.ClientAuthorizationInfo.AuthResult ?? new AuthorizationResult(false);
                        var result = context.Request.ClientAuthorizationInfo.AuthResult;

                        if (!result?.IsAuthorized ?? false)
                        {
                            context.Response.StatusCode = 403;
                            return false;
                        }
                    }
                    else
                    {
                        if (context.Request.ClientAuthorizationInfo == null)
                        {
                            context.Request.ClientAuthorizationInfo = new ClientAuthorization
                            {
                                AuthorizedBy = AuthorizationType.None,
                                AuthResult = new AuthorizationResult(true)
                            };
                        }
                        else
                        {
                            context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.None;
                            context.Request.ClientAuthorizationInfo.AuthResult = new AuthorizationResult(true);
                        }
                    }
                }
                else
                {
                    if (context.Request.ClientAuthorizationInfo == null)
                    {
                        context.Request.ClientAuthorizationInfo = new ClientAuthorization
                        {
                            AuthorizedBy = AuthorizationType.Anonymous,
                            AuthResult = new AuthorizationResult(true)
                        };
                    }
                    else
                    {
                        context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.Anonymous;
                        context.Request.ClientAuthorizationInfo.AuthResult = new AuthorizationResult(true);
                    }
                }

                return true;
            }

            return false;
        }
    }
}
