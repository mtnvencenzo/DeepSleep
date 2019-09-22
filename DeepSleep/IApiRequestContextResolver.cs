// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRequestContext.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace DeepSleep
{
    /// <summary></summary>
    public interface IApiRequestContextResolver
    {
        /// <summary>Gets the context.</summary>
        /// <returns></returns>
        ApiRequestContext GetContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IApiRequestContextResolver SetContext(ApiRequestContext context);
    }
}