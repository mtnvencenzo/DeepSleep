namespace DeepSleep.Validation
{
    using System;
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
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public async Task<bool> InvokeMethodValidation(MethodInfo method, ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter)
        {
            bool isValid = true;

            var attributes = method
                .GetCustomAttributes<TypeBasedValidatorAttribute>(true)
                .OrderBy(a => a.Order);

            foreach (var attribute in attributes)
            {
                IApiValidator validator = (serviceProvider != null)
                    ? serviceProvider.GetService(attribute.ValidatorType) as IApiValidator
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
                                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;
                                isValid = false;
                            }

                            if (result.Message != null)
                            {
                                context.ProcessingInfo.ExtendedMessages.Add(result.Message);
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
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public Task<bool> InvokeObjectValidation(object obj, ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter)
        {
            var source = new TaskCompletionSource<bool>();
            source.SetResult(true);
            return source.Task;
        }
    }
}
