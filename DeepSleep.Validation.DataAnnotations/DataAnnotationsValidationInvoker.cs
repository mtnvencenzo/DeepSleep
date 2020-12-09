namespace DeepSleep.Validation.DataAmmotations
{
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
        /// <returns></returns>
        public Task<bool> InvokeMethodValidation(MethodInfo method, ApiRequestContext context)
        {
            return Task.FromResult(true);
        }

        /// <summary>Invokes the object validation.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task<bool> InvokeObjectValidation(object obj, ApiRequestContext context)
        {
            if (obj == null)
            {
                return Task.FromResult(true);
            }

            var validationContext = new ValidationContext(
                obj, 
                serviceProvider: context.RequestServices, 
                items: null);
            
            var results = new List<ValidationResult>();


            // TODO: configure the true parameter here
            var isValid = Validator.TryValidateObject(obj, validationContext, results, true);
            if (!isValid)
            {
                context.ProcessingInfo.Validation.State = ApiValidationState.Failed;

                foreach (var validationResult in results)
                {
                    var message = validationResult.ErrorMessage;
                    context.ErrorMessages.Add(message);
                }
            }


            return Task.FromResult(isValid);
        }
    }
}
