namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomApiRouteErrorResponseProviderAttribute : ApiRouteErrorResponseProviderAttribute
    {
        /// <summary>Processes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        public override Task<object> Process(ApiRequestContext context, IList<string> errors)
        {
            return Task.FromResult(new CustomResponseErrorObject(errors) as object);
        }
    }
}
