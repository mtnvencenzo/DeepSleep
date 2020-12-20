namespace DeepSleep.Pipeline
{
    using DeepSleep.Validation;
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
            var context = contextResolver.GetContext();

            var validationProvider = context?.RequestServices?.GetService<IApiValidationProvider>();

            if (await context.ProcessHttpEndpointValidation(validationProvider).ConfigureAwait(false))
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
        /// <param name="validationProvider">The validation provider.</param>
        /// <returns></returns>
        internal static async Task<bool> ProcessHttpEndpointValidation(this ApiRequestContext context, IApiValidationProvider validationProvider)
        {
            if (!context.RequestAborted.IsCancellationRequested)
            {
                context.ProcessingInfo.Validation.State = ApiValidationState.Validating;

                if (validationProvider != null)
                {
                    var invokers = validationProvider
                        .GetInvokers()
                        .ToList();

                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo.InvocationContext.UriModel != null)
                        {
                            var objectUriValidationResult = await validationInvoker.InvokeObjectValidation(
                                context.RequestInfo.InvocationContext.UriModel,
                                context).ConfigureAwait(false);

                            if (!objectUriValidationResult)
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                            }
                        }
                    }


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo.InvocationContext.BodyModel != null)
                        {
                            try
                            {
                                var objectBodyValidationResult = await validationInvoker.InvokeObjectValidation(
                                    context.RequestInfo.InvocationContext.BodyModel,
                                    context).ConfigureAwait(false);

                                if (!objectBodyValidationResult)
                                {
                                    context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                                }
                            }
                            catch
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Exception;
                                throw;
                            }
                        }
                    }


                    foreach (var validationInvoker in invokers)
                    {
                        if (context.RequestInfo?.InvocationContext?.ControllerMethod != null)
                        {
                            try
                            {
                                var methodValidationResult = await validationInvoker.InvokeMethodValidation(
                                    context.RequestInfo.InvocationContext.ControllerMethod,
                                    context).ConfigureAwait(false);

                                if (!methodValidationResult)
                                {
                                    context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                                }
                            }
                            catch
                            {
                                context.ProcessingInfo.Validation.State = ApiValidationState.Exception;
                                throw;
                            }
                        }
                    }
                }


                if (context.ProcessingInfo.Validation.State == ApiValidationState.Failed)
                {
                    context.ResponseInfo.StatusCode = context.ProcessingInfo.Validation.SuggestedErrorStatusCode;
                    return false;
                }

                context.ProcessingInfo.Validation.State = ApiValidationState.Succeeded;
                return true;
            }


            return false;
        }
    }
}
