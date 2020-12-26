namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class TypeBasedValidationInvoker : IApiValidationInvoker
    {
        /// <summary>Invokes the method validation.</summary>
        /// <param name="method">The method.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task<bool> InvokeMethodValidation(MethodInfo method, ApiRequestContext context)
        {
            bool isValid = true;

            var statusCodePrecedence = new List<int>
            {
                401,
                403,
                404,
            };

            var attributes = method
                .GetCustomAttributes<TypeBasedValidatorAttribute>(true)
                .OrderBy(a => a.Order);

            foreach (var attribute in attributes)
            {
                IApiValidator validator = (context.RequestServices != null)
                    ? context.RequestServices.GetService(attribute.ValidatorType) as IApiValidator
                    : null;

                if (validator == null)
                {
                    validator = Activator.CreateInstance(attribute.ValidatorType) as IApiValidator;
                }


                if (validator != null)
                {
                    var results = await validator.Validate(new ApiValidationArgs
                    {
                        ApiContext = context,
                        ValiationState = context.ValidationState()
                    }).ConfigureAwait(false);

                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            if (!result.IsValid)
                            {
                                if (!string.IsNullOrWhiteSpace(result.Message))
                                {
                                    context.Validation.Errors.Add(result.Message);
                                }

                                context.Validation.State = ApiValidationState.Failed;
                                isValid = false;

                                // Update the suggested status if it applies
                                var suggestedStatus = result.SuggestedHttpStatusCode;
                                var currentStatus = context.Validation.SuggestedErrorStatusCode;

                                if (suggestedStatus != currentStatus)
                                {
                                    if (statusCodePrecedence?.Contains(suggestedStatus) == true && statusCodePrecedence?.Contains(currentStatus) == false)
                                    {
                                        context.Validation.SuggestedErrorStatusCode = suggestedStatus;
                                    }
                                    else if (statusCodePrecedence?.Contains(suggestedStatus) == true && statusCodePrecedence?.Contains(currentStatus) == true)
                                    {
                                        if (statusCodePrecedence.IndexOf(suggestedStatus) < statusCodePrecedence.IndexOf(currentStatus))
                                        {
                                            context.Validation.SuggestedErrorStatusCode = suggestedStatus;
                                        }
                                    }
                                    else if (statusCodePrecedence?.Contains(suggestedStatus) == false && statusCodePrecedence?.Contains(currentStatus) == false)
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

            return isValid;
        }

        /// <summary>Invokes the object validation.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task<bool> InvokeObjectValidation(object obj, ApiRequestContext context)
        {
            return Task.FromResult(true);
        }
    }
}
