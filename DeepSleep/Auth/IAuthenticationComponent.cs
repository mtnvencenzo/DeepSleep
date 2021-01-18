namespace DeepSleep.Auth
{
    using System;

    /// <summary>
    /// Registration component to configure an authentication provider.
    /// </summary>
    /// <remarks>
    /// When specifing on an individual route, default request configured authentication providers will be excluded and only the authentication 
    /// providers specified on the individual route will be used.
    /// <para>
    /// If the authentication provider fails authenticating the request, a 401 Unauthorized response will be returned.
    /// </para>
    /// <para>
    /// Authentication providers are bypassed if the request is configured for anonymous access. <seealso cref="DeepSleep.Configuration.IApiRequestConfiguration.AllowAnonymous" />. 
    /// Unless specifically configured, the default for anonymous access is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="DeepSleep.Auth.IAuthenticationComponent" />
    public interface IAuthenticationComponent : IAuthenticationProvider
    {
        /// <summary>Activates the <see cref="DeepSleep.Auth.IAuthenticationProvider" />.</summary>
        /// <remarks>
        /// For activation to be successful, the <see cref="IAuthenticationProvider" /> must contain a public parameterless constructor
        /// or be accessible via DI using <see cref="IServiceProvider"/>
        /// </remarks>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="DeepSleep.Auth.IAuthenticationProvider" /></returns>
        IAuthenticationProvider Activate(IApiRequestContextResolver contextResolver);
    }
}
