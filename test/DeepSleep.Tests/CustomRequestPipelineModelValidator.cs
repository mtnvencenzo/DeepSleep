namespace DeepSleep.Tests
{
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomRequestPipelineModelValidator : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(IApiRequestContextResolver contextResolver)
        {
            return Task.FromResult(ApiValidationResult.Single("VALIDATOR-1"));
        }
    }
}
