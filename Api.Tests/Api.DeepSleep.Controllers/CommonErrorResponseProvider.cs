namespace Api.DeepSleep.Controllers
{
    using Api.DeepSleep.Models;
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Validation.IValidationErrorResponseProvider" />
    public class CommonErrorResponseProvider : IValidationErrorResponseProvider
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
                    .Select(e => new ErrorMessage { ErrorMessageStr = e })
                    .ToList();

                if (messages.Count > 0)
                {
                    return Task.FromResult(new CommonErrorResponse
                    {
                        Messages = messages
                    } as object);
                }
            }

            return Task.FromResult(null as object);
        }
    }
}
