namespace DeepSleep.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Validation.IValidationErrorResponseProvider" />
    public class ValidationErrorResponseProvider : IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public Task<object> Process(ApiRequestContext context, IList<string> errors)
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
    }
}
