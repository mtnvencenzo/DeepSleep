namespace DeepSleep.Auth
{
    /// <summary>
    /// Registration component to configure an authorization provider.
    /// Multiple attributes are allowed to enable multiple providers.
    /// </summary>
    /// <remarks>
    /// When specifing on an individual route, default request configured authorization providers will be excluded and only the authorization 
    /// providers specified on the individual route will be used.
    /// <para>
    /// Each authorization provider defined on a route will be required to successfully authorize the request.
    /// If at least one authorization provider fails authorization, the request will be responded to with a 403 Forbidden response.
    /// </para>
    /// <para>
    /// Authorization providers are bypassed if the request is configured for anonymous access. <seealso cref="DeepSleep.Configuration.IDeepSleepRequestConfiguration.AllowAnonymous" />. 
    /// Unless specifically configured, the default for anonymous access is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="DeepSleep.Auth.IAuthorizationProvider" />
    public interface IAuthorizationComponent : IAuthorizationProvider
    {
    }
}
