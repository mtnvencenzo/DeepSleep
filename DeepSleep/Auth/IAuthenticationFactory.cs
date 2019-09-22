// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthFactory.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace DeepSleep.Auth
{
    /// <summary></summary>
    public interface IAuthenticationFactory
    {
        /// <summary>Gets the provider.</summary>
        /// <param name="type">The type.</param>
        /// <returns>The <see cref="IAuthenticationProvider"/>.</returns>
        IAuthenticationProvider GetProvider(string type);

        /// <summary>Fors the each.</summary>
        /// <returns></returns>
        IEnumerable<IAuthenticationProvider> GetProviders();

        /// <summary>Adds a provider to the factory</summary>
        /// <param name="provider">The providder being added</param>
        /// <returns></returns>
        IAuthenticationFactory AddProvider(IAuthenticationProvider provider);

        /// <summary>Firsts the or default.</summary>
        /// <returns></returns>
        IAuthenticationProvider FirstOrDefault();
    }
}