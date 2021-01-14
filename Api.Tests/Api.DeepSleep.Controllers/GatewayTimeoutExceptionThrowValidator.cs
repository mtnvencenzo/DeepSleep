namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GatewayTimeoutExceptionThrowValidator : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            throw new ApiGatewayTimeoutException();
        }
    }
}
