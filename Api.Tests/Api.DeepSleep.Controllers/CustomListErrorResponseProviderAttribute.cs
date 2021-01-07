namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CustomListErrorResponseProviderAttribute : ApiRouteErrorResponseProviderAttribute
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public override Task<object> Process(ApiRequestContext context, IList<string> errors)
        {
            if ((errors?.Count ?? 0) > 0)
            {
                var messages = errors
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct()
                    .OrderBy(e => e)
                    .ToList();

                return Task.FromResult(messages as object);
            }

            return Task.FromResult(null as object);
        }
    }
}
