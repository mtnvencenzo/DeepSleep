namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.ApiRouteErrorResponseProviderAttribute" />
    public class CustomListErrorResponseProviderAttribute : ApiRouteErrorResponseProviderAttribute
    {
        /// <summary>Processes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public override Task<object> Process(IApiRequestContextResolver contextResolver, IList<string> errors)
        {
            if ((errors?.Count ?? 0) > 0)
            {
                var messages = errors
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct()
                    .ToList();

                return Task.FromResult(messages as object);
            }

            return Task.FromResult(null as object);
        }

        /// <summary>Gets the type of the error.</summary>
        /// <returns></returns>
        public override Type GetErrorType() => typeof(List<string>);
    }
}
