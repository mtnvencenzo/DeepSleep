﻿namespace DeepSleep.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary></summary>
    public interface IEndpointValidator
    {
        /// <summary>Validates the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        Task<IList<ApiValidationResult>> Validate(ApiValidationArgs args);
    }
}