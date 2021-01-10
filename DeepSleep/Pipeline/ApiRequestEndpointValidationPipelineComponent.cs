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
                var statusCodePrecedence = new List<int>
                {
                    401,
                    403,
                    404,
                };

                foreach (var validator in context.Configuration.Validators.OrderBy(v => v.Order))
                {
                    // Skip validators that are not set to run if validation != failed
                    if (context.Validation.State == ApiValidationState.Failed && validator.Continuation == ValidationContinuation.OnlyIfValid)
                    {
                        continue;
                    }

                    IEnumerable <ApiValidationResult> results = null;

                    try
                    {
                        results = await validator.Validate(new ApiValidationArgs
                        {
                            ApiContext = context
                        }).ConfigureAwait(false);
                    }
                    catch
                    {
                        context.Validation.State = ApiValidationState.Exception;
                        throw;
                    }

                    foreach (var result in (results ?? new List<ApiValidationResult>()).Where(r => r != null))
                    {
                        if (!result.IsValid)
                        {
                            context.Validation.State = ApiValidationState.Failed;
                            context.AddValidationError(result.Message);

                            var suggestedStatus = result.SuggestedHttpStatusCode ?? 400;
                            var currentStatus = context.Validation.SuggestedErrorStatusCode;

                            if (suggestedStatus != currentStatus)
                            {
                                if (statusCodePrecedence.Contains(suggestedStatus) == true && statusCodePrecedence.Contains(currentStatus) == false)
                                {
                                    context.Validation.SuggestedErrorStatusCode = suggestedStatus;
                                }
                                else if (statusCodePrecedence.Contains(suggestedStatus) == true && statusCodePrecedence.Contains(currentStatus) == true)
                                {
                                    if (statusCodePrecedence.IndexOf(suggestedStatus) < statusCodePrecedence.IndexOf(currentStatus))
                                    {
                                        context.Validation.SuggestedErrorStatusCode = suggestedStatus;
                                    }
                                }
                                else if (statusCodePrecedence.Contains(suggestedStatus) == false && statusCodePrecedence.Contains(currentStatus) == false)
                                {
                                    if (suggestedStatus > currentStatus)
                                    {
                                        context.Validation.SuggestedErrorStatusCode = suggestedStatus;
                                    }
                                }
                            }
                        }
                    }
                }

                if (context.Validation.State == ApiValidationState.Failed)
                {
                    context.Response.StatusCode = context.Validation.SuggestedErrorStatusCode;
                    return false;
                }

                context.Validation.State = ApiValidationState.Succeeded;
                return true;
            }


            return false;
        }
    }
}
