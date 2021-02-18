namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

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
    /// <typeparam name="T"><see cref="DeepSleep.Auth.IAuthorizationProvider" /></typeparam>
    /// <seealso cref="DeepSleep.Auth.IAuthorizationComponent" />
    public class ApiAuthorizationComponent<T> : IAuthorizationComponent where T : IAuthorizationProvider
    {
        /// <summary>Authorizes the request.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public virtual async Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver)
        {
            var provider = this.Activate(contextResolver);
            if (provider != null)
            {
                return await provider.Authorize(contextResolver).ConfigureAwait(false);
            }

            return null;
        }

        /// <summary>Activates the <see cref="DeepSleep.Auth.IAuthorizationProvider" />.</summary>
        /// <remarks>
        /// For activation to be successful, the <see cref="IAuthorizationProvider" /> must contain a public parameterless constructor
        /// or be accessible via DI using <see cref="IServiceProvider"/>
        /// </remarks>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="DeepSleep.Auth.IAuthorizationProvider" /></returns>
        protected virtual IAuthorizationProvider Activate(IApiRequestContextResolver contextResolver)
        {
            IAuthorizationProvider provider = null;

            var context = contextResolver?.GetContext();
            if (context != null)
            {
                try
                {
                    if (context.RequestServices != null)
                    {
                        provider = context.RequestServices.GetService(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthorizationProvider;
                    }
                }
                catch { }

                if (provider == null)
                {
                    try
                    {
                        provider = Activator.CreateInstance(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthorizationProvider;
                    }
                    catch { }
                }
            }

            return provider;
        }
    }
}
