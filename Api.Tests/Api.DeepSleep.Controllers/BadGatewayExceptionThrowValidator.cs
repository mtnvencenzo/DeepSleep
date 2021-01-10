namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BadGatewayExceptionThrowValidator : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            throw new ApiBadGatewayException();
        }
    }
}
