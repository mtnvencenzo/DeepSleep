namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class DataAnnotationsValidationInvoker : IApiValidationInvoker
    {
        /// <summary>Invokes the method validation.</summary>
        /// <param name="method">The method.</param>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns></returns>
        public Task<bool> InvokeMethodValidation(MethodInfo method, ApiRequestContext context, IServiceProvider serviceProvider, IApiResponseMessageConverter responseMessageConverter)
        {
            var source = new TaskCompletionSource<bool>();
            source.SetResult(true);
            return source.Task;
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
            if (obj == null)
            {
                source.SetResult(true);
                return source.Task;
            }

            
            var validationContext = new ValidationContext(obj, serviceProvider: serviceProvider, items: null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, results, true);
            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    var message = responseMessageConverter.Convert(validationResult.ErrorMessage);
                    context.ProcessingInfo.ExtendedMessages.Add(message);
                }
            }


            source.SetResult(isValid);
            return source.Task;
        }
    }
}
