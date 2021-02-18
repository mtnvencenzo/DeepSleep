namespace DeepSleep.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Validation.IValidationErrorResponseProvider" />
    public class ValidationErrorResponseProvider : IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public Task<object> Process(IApiRequestContextResolver contextResolver, IList<string> errors)
        {
            if ((errors?.Count ?? 0) > 0)
            {
                var messages = errors
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct()
                    .ToList();

                if (messages.Count > 0)
                {
                    return Task.FromResult(messages as object);
                }
            }

            return Task.FromResult(null as object);
        }

        /// <summary>Gets the type of the error.</summary>
        /// <returns></returns>
        public Type GetErrorType() => typeof(List<string>);
    }
}
