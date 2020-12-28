﻿namespace Api.DeepSleep.Controllers
{
    using Api.DeepSleep.Models;
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiErrorResponseProvider" />
    public class CommonErrorResponseProvider : IValidationErrorResponseProvider
    {
        /// <summary>Processes the specified errors.</summary>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public Task<object> Process(IList<string> errors)
        {
            if ((errors?.Count ?? 0) > 0)
            {
                var messages = errors
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct()
                    .Select(e => BuildResponseMessageFromResource(e))
                    .Where(e => e != null)
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

        /// <summary>Builds the response message from resource.</summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        private static ErrorMessage BuildResponseMessageFromResource(string resource)
        {
            string[] resourceParts = resource.Split(new[] { '|' });

            if (resourceParts.Length == 2)
            {
                return new ErrorMessage
                {
                    ErrorCode = resourceParts[0],
                    ErrorMessageStr = resourceParts[1]?.Trim()
                };
            }
            else if (resourceParts.Length == 1)
            {
                return new ErrorMessage
                {
                    ErrorCode = resourceParts[0],
                    ErrorMessageStr = null
                };
            }

            return null;
        }
    }
}
