﻿namespace Api.DeepSleep.NetCore3_1
{
    using Api.DeepSleep.Models;
    using global::DeepSleep;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiErrorResponseProvider" />
    public class CommonErrorResponseProvider : IApiErrorResponseProvider
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        /// <summary>Initializes a new instance of the <see cref="CommonErrorResponseProvider"/> class.</summary>
        /// <param name="apiRequestContextResolver">The API request context resolver.</param>
        public CommonErrorResponseProvider(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Process(ApiRequestContext context)
        {
            var injectedContext = apiRequestContextResolver.GetContext();

            if (injectedContext?.ResponseInfo != null && injectedContext.ResponseInfo.HasSuccessStatus() == false)
            {
                if (injectedContext.ErrorMessages != null && injectedContext.ErrorMessages.Count > 0)
                {
                    var messages = injectedContext.ErrorMessages
                        .Where(e => !string.IsNullOrWhiteSpace(e))
                        .Select(e => BuildResponseMessageFromResource(e))
                        .Where(e => e != null)
                        .ToList();

                    if (messages.Count > 0)
                    {
                        injectedContext.ResponseInfo.ResponseObject = new CommonErrorResponse
                        {
                            Messages = messages
                        };
                    }
                }
            }

            return Task.CompletedTask;
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
