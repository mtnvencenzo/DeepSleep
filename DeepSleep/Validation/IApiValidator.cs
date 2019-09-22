// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApiValidator.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepSleep.Validation
{
    /// <summary></summary>
    public interface IApiValidator
    {
        /// <summary>Validates the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        Task<IEnumerable<ApiValidationResult>> Validate(ApiValidationArgs args);
    }
}