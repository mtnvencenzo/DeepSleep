namespace DeepSleep.Pipeline
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiResponseCorsPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponseCorsPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiResponseCorsPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            try
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
            finally
            {
                var context = contextResolver.GetContext();
                await context.ProcessHttpResponseCrossOriginResourceSharing().ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiResponseCorsPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API response cors.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiResponseCors(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiResponseCorsPipelineComponent>();
        }

        /// <summary>Processes the HTTP response cross origin resource sharing.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static Task<bool> ProcessHttpResponseCrossOriginResourceSharing(this ApiRequestContext context)
        {
            if (!(context?.RequestAborted.IsCancellationRequested ?? false))
            {
                // No Cors for not found endponts or method not found
                if (context.Runtime.Internals.IsNotFound || context.Runtime.Internals.IsMethodNotFound)
                {
                    return Task.FromResult(true);
                }

                if (context.Request?.CrossOriginRequest?.Origin != null)
                {
                    var allowedOrigins = (context.Configuration?.CrossOriginConfig?.AllowedOrigins ?? new string[] { })
                        .Distinct()
                        .Where(i => !string.IsNullOrWhiteSpace(i))
                        .Select(i => i.Trim())
                        .ToList();

                    var exposeHeaders = (context.Configuration?.CrossOriginConfig?.ExposeHeaders ?? new string[] { })
                        .Distinct()
                        .Where(i => !string.IsNullOrWhiteSpace(i))
                        .Select(i => i.Trim())
                        .ToList();

                    var allowCredentials = context.Configuration?.CrossOriginConfig?.AllowCredentials;

                    string allowedOrigin = null;

                    if (allowedOrigins.Count > 0 && allowedOrigins.Contains("*"))
                    {
                        allowedOrigin = context.Request.CrossOriginRequest.Origin;
                    }
                    else
                    {
                        allowedOrigin = allowedOrigins
                            .Where(i => i.Equals(context.Request.CrossOriginRequest.Origin, StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault();
                    }

                    context.Response.AddHeader("Access-Control-Allow-Origin", allowedOrigin ?? string.Empty);
                    context.Response.AddHeader("Access-Control-Allow-Credentials", (allowCredentials ?? false).ToString().ToLowerInvariant());

                    if (exposeHeaders.Count > 0)
                    {
                        context.Response.AddHeader("Access-Control-Expose-Headers", string.Join(", ", exposeHeaders));
                    }

                    context.Response.AddHeader("Vary", "Origin");
                }

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
