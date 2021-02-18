namespace DeepSleep.Pipeline
{
    using DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiRequestEndpointValidationPipelineComponent : PipelineComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiRequestEndpointValidationPipelineComponent"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ApiRequestEndpointValidationPipelineComponent(ApiRequestDelegate next)
            : base(next) { }

        /// <summary>Invokes the specified context resolver.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        /// <returns></returns>
        public override async Task Invoke(IApiRequestContextResolver contextResolver)
        {
            var context = contextResolver
                 .GetContext()
                 .SetThreadCulure();

            var beforePipelines = context?.Configuration?.PipelineComponents?
                .Where(p => p.Placement == PipelinePlacement.BeforeEndpointValidation)
                .OrderBy(p => p.Order)
                .ToList() ?? new List<IRequestPipelineComponent>();

            foreach (var pipeline in beforePipelines)
            {
                await pipeline.Invoke(contextResolver).ConfigureAwait(false);
            }

            if (await context.ProcessHttpEndpointValidation().ConfigureAwait(false))
            {
                await apinext.Invoke(contextResolver).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class ApiRequestEndpointValidationPipelineComponentExtensionMethods
    {
        /// <summary>Uses the API request endpoint validation.</summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public static IApiRequestPipeline UseApiRequestEndpointValidation(this IApiRequestPipeline pipeline)
        {
            return pipeline.UsePipelineComponent<ApiRequestEndpointValidationPipelineComponent>();
        }

        /// <summary>Processes the HTTP endpoint validation.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpEndpointValidation(this ApiRequestContext context)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                context.Validation.State = ApiValidationState.Validating;

                var validationProviders = (context.RequestServices?.GetServices(typeof(IApiValidationProvider)) ?? new List<IApiValidationProvider>())
                    .Where(p => p as IApiValidationProvider != null)
                    .Select(p => p as IApiValidationProvider)
                    .OrderBy(p => p.Order);

                var contextResolver = context.RequestServices.GetService<IApiRequestContextResolver>();

                foreach (var validationProvider in validationProviders)
                {
                    await validationProvider.Validate(contextResolver).ConfigureAwait(false);
                }

                if (context.Validation.State == ApiValidationState.Failed)
                {
                    var defaultStatusCode = context.Configuration?.ValidationErrorConfiguration?.BodyValidationErrorStatusCode ?? 400;
                    context.Response.StatusCode = context.Validation.SuggestedErrorStatusCode ?? defaultStatusCode;
                    return false;
                }

                context.Validation.State = ApiValidationState.Succeeded;
                return true;
            }


            return false;
        }
    }
}
