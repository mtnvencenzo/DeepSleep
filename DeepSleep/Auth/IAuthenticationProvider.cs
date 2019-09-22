// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthProvider.cs" company="Ronaldo Vecchi">
//   Copyright © Ronaldo Vecchi
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace DeepSleep.Auth
{
    /// <summary></summary>
    public interface IAuthenticationProvider
    {
        #region Public Methods and Operators

        /// <summary>Authenticates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="responseMessageConverter">The response message converter.</param>
        /// <returns>The <see cref="Task" />.</returns>
        Task<AuthenticationResult> Authenticate(ApiRequestContext context, IApiResponseMessageConverter responseMessageConverter);

        /// <summary>Determines whether this instance [can handle authentication scheme] the specified scheme.</summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        bool CanHandleAuthScheme(string scheme);

        #endregion

        /// <summary>Gets the realm.</summary>
        /// <value>The realm.</value>
        string Realm { get; }

        /// <summary>Gets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        string AuthScheme { get; }
    }
}