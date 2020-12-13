﻿namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep;
    using global::DeepSleep.Validation;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class GatewayTimeoutExceptionThrowValidator : IApiValidator
    {
        public Task<IEnumerable<ApiValidationResult>> Validate(ApiValidationArgs args)
        {
            throw new ApiGatewayTimeoutException();
        }
    }
}