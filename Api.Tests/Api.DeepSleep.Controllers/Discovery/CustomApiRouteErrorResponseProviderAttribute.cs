namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomApiRouteErrorResponseProviderAttribute : ApiRouteErrorResponseProviderAttribute
    {
        /// <summary>Processes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public override Task<object> Process(IApiRequestContextResolver contextResolver, IList<string> errors)
        {
            return Task.FromResult(new CustomResponseErrorObject(errors) as object);
        }
    }
}
