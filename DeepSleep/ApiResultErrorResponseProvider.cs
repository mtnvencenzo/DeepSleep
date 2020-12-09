namespace DeepSleep
{
    using System;
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
            if (context?.ResponseInfo != null && context.ResponseInfo.HasSuccessStatus() == false)
            {
                if (context.ErrorMessages != null && context.ErrorMessages.Count > 0)
                {
                    if (this.WriteToBody && context.ResponseInfo.ResponseObject == null)
                    {
                        var messages = context.ErrorMessages
                            .Where(e => !string.IsNullOrWhiteSpace(e))
                            .Select(e => BuildResponseMessageFromResource(e))
                            .Where(e => e != null)
                            .ToList();

                        if (messages.Count > 0)
                        {
                            context.ResponseInfo.ResponseObject = new ApiResult
                            {
                                Messages = messages
                            };
                        }
                    }

                    if (this.WriteToHeaders)
                    {
                        var messages = context.ErrorMessages
                            .Where(e => !string.IsNullOrWhiteSpace(e))
                            .ToList();

                        if (messages.Count > 0)
                        {
                            messages.ForEach(m => context.ResponseInfo.AddHeader("X-Api-Message", m));
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        /// <summary>Gets or sets a value indicating whether [write to body].</summary>
        /// <value><c>true</c> if [write to body]; otherwise, <c>false</c>.</value>
        public bool WriteToBody { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether [write to headers].</summary>
        /// <value><c>true</c> if [write to headers]; otherwise, <c>false</c>.</value>
        public bool WriteToHeaders { get; set; } = false;

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
