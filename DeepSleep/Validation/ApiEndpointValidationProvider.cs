namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Validation.IApiValidationProvider" />
    public class ApiEndpointValidationProvider : IApiValidationProvider
    {
        /// <summary>Gets the order.</summary>
        /// <value>The order.</value>
        public int Order => 10;

        /// <summary>Validates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Validate(ApiRequestContext context)
        {
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

                IEnumerable<ApiValidationResult> results = null;

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

                var defaultStatusCode = context.Configuration?.ValidationErrorConfiguration?.BodyValidationErrorStatusCode ?? 400;

                foreach (var result in (results ?? new List<ApiValidationResult>()).Where(r => r != null))
                {
                    if (!result.IsValid)
                    {
                        context.Validation.State = ApiValidationState.Failed;
                        context.AddValidationError(result.Message);



                        var suggestedStatus = result.SuggestedHttpStatusCode ?? defaultStatusCode;
                        var currentStatus = context.Validation.SuggestedErrorStatusCode ?? defaultStatusCode;

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
        }
    }
}
