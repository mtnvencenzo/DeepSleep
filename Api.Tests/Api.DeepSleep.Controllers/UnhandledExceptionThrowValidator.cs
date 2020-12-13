namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Validation;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UnhandledExceptionThrowValidator : IApiValidator
    {
        public Task<IEnumerable<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            throw new Exception();
        }
    }
}
