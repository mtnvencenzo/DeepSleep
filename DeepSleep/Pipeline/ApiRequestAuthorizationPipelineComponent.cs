namespace DeepSleep.Pipeline
{
    using DeepSleep.Auth;
    using System.Collections.Generic;
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
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

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
                context.Request.ClientAuthorizationInfo = context.Request.ClientAuthorizationInfo ?? new ClientAuthorization();

                if (!(context.Configuration?.AllowAnonymous ?? false))
                {
                    var authorizationComponents = context.Configuration?.AuthorizationProviders ?? new List<IAuthorizationComponent>();

                    var providers = authorizationComponents
                        .Where(a => a != null)
                        .Select(a => a.Activate(context))
                        .ToList();

                    foreach (var authProvider in providers)
                    {
                        context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.Provider;
                        var result = await authProvider.Authorize(context).ConfigureAwait(false);

                        if (result == null)
                        {
                            result = new AuthorizationResult(false);
                        }

                        result.Policy = authProvider.Policy;
                        context.Request.ClientAuthorizationInfo.AuthResults.Add(result);
                    }

                    if (context.Request.ClientAuthorizationInfo.AuthResults.Count == 0)
                    {
                        context.Request.ClientAuthorizationInfo.AuthResults.Add(new AuthorizationResult(true));
                        context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.None;
                    }

                    var isFailed = context.Request.ClientAuthorizationInfo.AuthResults.Any(a => a.IsAuthorized == false);
                    if (isFailed)
                    {
                        context.Response.StatusCode = 403;
                        return false;
                    }
                }
                else
                {
                    context.Request.ClientAuthorizationInfo.AuthResults.Add(new AuthorizationResult(true));
                    context.Request.ClientAuthorizationInfo.AuthorizedBy = AuthorizationType.Anonymous;
                }

                return true;
            }

            return false;
        }
    }
}
