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
            var source = new TaskCompletionSource<bool>();
            if (obj == null)
            {
                source.SetResult(true);
                return source.Task;
            }

            var validationContext = new ValidationContext(
                obj, 
                serviceProvider: context.RequestServices, 
                items: null);
            
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(obj, validationContext, results, true);
            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    var message = validationResult.ErrorMessage;
                    context.ErrorMessages.Add(message);
                }
            }


            source.SetResult(isValid);
            return source.Task;
        }
    }
}
