namespace DeepSleep
{
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.IApiErrorResponseProvider" />
    public class ApiResultErrorResponseProvider : IApiErrorResponseProvider
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Process(ApiRequestContext context)
        {
            if (context?.Response != null && context.Response.HasSuccessStatus() == false)
            {
                if (context.Validation?.Errors != null && context.Validation?.Errors.Count > 0)
                {
                    if (context.Response.ResponseObject == null)
                    {
                        var messages = context.Validation.Errors
                            .Where(e => !string.IsNullOrWhiteSpace(e))
                            .Select(e => BuildResponseMessageFromResource(e))
                            .Where(e => e != null)
                            .ToList();

                        if (messages.Count > 0)
                        {
                            context.Response.ResponseObject = new ApiResult
                            {
                                Messages = messages
                            };
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>Builds the response message from resource.</summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        private static ApiResultMessage BuildResponseMessageFromResource(string resource)
        {
            string[] resourceParts = resource.Split(new[] { '|' });

            if (resourceParts.Length == 2)
            {
                return new ApiResultMessage
                {
                    Code = resourceParts[0],
                    Message = resourceParts[1]?.Trim()
                };
            }
            else if (resourceParts.Length == 1)
            {
                return new ApiResultMessage
                {
                    Code = resourceParts[0],
                    Message = null
                };
            }

            return null;
        }
    }
}
