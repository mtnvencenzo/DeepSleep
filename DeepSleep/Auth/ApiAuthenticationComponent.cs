namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

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
    /// Authentication providers are bypassed if the request is configured for anonymous access. <seealso cref="DeepSleep.Configuration.IDeepSleepRequestConfiguration.AllowAnonymous" />. 
    /// Unless specifically configured, the default for anonymous access is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <typeparam name="T"><see cref="DeepSleep.Auth.IAuthenticationProvider" /></typeparam>
    /// <seealso cref="DeepSleep.Auth.IAuthenticationComponent" />
    public class ApiAuthenticationComponent<T> : IAuthenticationComponent where T : IAuthenticationProvider
    {
        private IAuthenticationProvider provider;

        string IAuthenticationProvider.Realm { get; }
        string IAuthenticationProvider.Scheme { get; }

        /// <summary>Activates the <see cref="DeepSleep.Auth.IAuthenticationProvider" />.</summary>
        /// <remarks>
        /// For activation to be successful, the <see cref="IAuthenticationProvider" /> must contain a public parameterless constructor
        /// or be accessible via DI using <see cref="IServiceProvider"/>
        /// </remarks>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="DeepSleep.Auth.IAuthenticationProvider" /></returns>
        public virtual IAuthenticationProvider Activate(IApiRequestContextResolver contextResolver)
        {
            if (this.provider == null)
            {
                var context = contextResolver?.GetContext();
                if (context != null)
                {
                    try
                    {
                        if (context.RequestServices != null)
                        {
                            this.provider = context.RequestServices.GetService(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthenticationProvider;
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Log(ex.ToString());
                    }

                    if (this.provider == null)
                    {
                        try
                        {
                            this.provider = Activator.CreateInstance(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthenticationProvider;
                        }
                        catch (Exception ex)
                        {
                            context.Log(ex.ToString());
                        }
                    }
                }
            }

            return this.provider;
        }

        /// <summary>Authenticates the request.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public virtual async Task<AuthenticationResult> Authenticate(IApiRequestContextResolver contextResolver)
        {
            var provider = this.Activate(contextResolver);
            if (provider != null)
            {
                return await provider.Authenticate(contextResolver).ConfigureAwait(false);
            }

            return null;
        }

        bool IAuthenticationProvider.CanHandleAuthScheme(string scheme)
        {
            return false;
        }
    }
}
