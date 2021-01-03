namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomApiRouteErrorResponseProviderAttribute : ApiRouteErrorResponseProviderAttribute
    {
        public override Task<object> Process(IList<string> errors)
        {
            return Task.FromResult(new CustomResponseErrorObject(errors) as object);
        }
    }
}
