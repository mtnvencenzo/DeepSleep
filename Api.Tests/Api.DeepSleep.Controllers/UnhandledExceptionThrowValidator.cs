namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UnhandledExceptionThrowValidator : IEndpointValidator
    {
        public Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            throw new Exception();
        }
    }
}
