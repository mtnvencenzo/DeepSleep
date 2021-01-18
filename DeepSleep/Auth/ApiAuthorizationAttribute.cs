namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute to configure an authorization provider for a specific route.
    /// Multiple attributes are allowed to enable multiple providers for a specific route.
    /// </summary>
    /// <remarks>
    /// When using attribute defined authorization providers, default request configured authorization providers will be excluded and only the authorization 
    /// providers specified via attributes will be enabled for the route.
    /// <para>
    /// Each authorization provider defined on a route will be required to successfully authorize the request.
    /// If at least one authorization provider fails authorization, the request will be responded to with a 403 Forbidden response.
    /// </para>
    /// <para>
    /// Authorization providers are bypassed if the request is configured for anonymous access. <seealso cref="DeepSleep.ApiRouteAllowAnonymousAttribute" />. 
    /// Unless specifically configured, the default for anonymous access is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="DeepSleep.Auth.IAuthorizationComponent" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthorizationAttribute : Attribute, IAuthorizationComponent
    {
        /// <summary>Initializes a new instance of the <see cref="ApiAuthorizationAttribute"/> class.</summary>
        /// <param name="authorizationProviderType">The Type representing the <see cref="DeepSleep.Auth.IAuthorizationProvider" /> that will perform the authorization.</param>
        /// <exception cref="ArgumentNullException">authorizationProviderType</exception>
        /// <exception cref="ArgumentException">authorizationProviderType</exception>
        public ApiAuthorizationAttribute(Type authorizationProviderType)
        {
            if (authorizationProviderType == null)
            {
                throw new ArgumentNullException(nameof(authorizationProviderType));
            }

            if (authorizationProviderType.GetInterface(nameof(IAuthorizationProvider), false) == null)
            {
                throw new ArgumentException($"{nameof(authorizationProviderType)} must implement interface {typeof(IAuthorizationProvider).AssemblyQualifiedName}");
            }

            this.AuthorizationProviderType = authorizationProviderType;
        }

        /// <summary>The Type representing the <see cref="DeepSleep.Auth.IAuthorizationProvider" /> that will perform the authorization.</summary>
        /// <value>The type of the authentication provider.</value>
        public Type AuthorizationProviderType { get; }

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

        /// <summary>Activates the <see cref="IAuthorizationProvider" />.</summary>
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
                        provider = context.RequestServices.GetService(Type.GetType(AuthorizationProviderType.AssemblyQualifiedName)) as IAuthorizationProvider;
                    }
                }
                catch { }

                if (provider == null)
                {
                    try
                    {
                        provider = Activator.CreateInstance(Type.GetType(AuthorizationProviderType.AssemblyQualifiedName)) as IAuthorizationProvider;
                    }
                    catch { }
                }
            }

            return provider;
        }
    }
}
