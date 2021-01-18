namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Attribute to configure an authentication provider for a specific route.
    /// Multiple attributes are allowed to enable multiple providers for a specific route.
    /// </summary>
    /// <remarks>
    /// When using attribute defined authentication providers, default request configured authentication providers will be excluded and only the authentication 
    /// providers specified via attributes will be enabled for the route.
    /// <para>
    /// If the authentication provider fails authenticating the request, a 401 Unauthorized response will be returned.
    /// </para>
    /// <para>
    /// Authentication providers are bypassed if the request is configured for anonymous access. <seealso cref="DeepSleep.ApiRouteAllowAnonymousAttribute" />. 
    /// Unless specifically configured, the default for anonymous access is <c>false</c>.
    /// </para>
    /// </remarks>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="DeepSleep.Auth.IAuthenticationComponent" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthenticationAttribute : Attribute, IAuthenticationComponent
    {
        private IAuthenticationProvider provider;

        string IAuthenticationProvider.Realm { get; }
        string IAuthenticationProvider.Scheme { get; }

        /// <summary>Initializes a new instance of the <see cref="ApiAuthenticationAttribute"/> class.</summary>
        /// <param name="authenticationProviderType">The Type representing the <see cref="DeepSleep.Auth.IAuthenticationProvider" /> that will perform the authentication.</param>
        /// <exception cref="ArgumentNullException">authenticationProviderType</exception>
        /// <exception cref="ArgumentException">authenticationProviderType</exception>
        public ApiAuthenticationAttribute(Type authenticationProviderType)
        {
            if (authenticationProviderType == null)
            {
                throw new ArgumentNullException(nameof(authenticationProviderType));
            }

            if (authenticationProviderType.GetInterface(nameof(IAuthenticationProvider), false) == null)
            {
                throw new ArgumentException($"{nameof(authenticationProviderType)} must implement interface {typeof(IAuthenticationProvider).AssemblyQualifiedName}");
            }

            this.AuthenticationProviderType = authenticationProviderType;
        }

        /// <summary>The Type representing the <see cref="DeepSleep.Auth.IAuthenticationProvider" /> that will perform the authentication.</summary>
        /// <value>The type of the authentication provider.</value>
        public Type AuthenticationProviderType { get; }

        /// <summary>Authenticates the request.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public async Task<AuthenticationResult> Authenticate(IApiRequestContextResolver contextResolver)
        {
            var provider = this.Activate(contextResolver);
            if (provider != null)
            {
                return await provider.Authenticate(contextResolver).ConfigureAwait(false);
            }

            return null;
        }

        /// <summary>Activates the <see cref="DeepSleep.Auth.IAuthenticationProvider" />.</summary>
        /// <remarks>
        /// For activation to be successful, the <see cref="IAuthenticationProvider" /> must contain a public parameterless constructor
        /// or be accessible via DI using <see cref="IServiceProvider"/>
        /// </remarks>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns>The <see cref="DeepSleep.Auth.IAuthenticationProvider" /></returns>
        public IAuthenticationProvider Activate(IApiRequestContextResolver contextResolver)
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
                            this.provider = context.RequestServices.GetService(Type.GetType(AuthenticationProviderType.AssemblyQualifiedName)) as IAuthenticationProvider;
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
                            this.provider = Activator.CreateInstance(Type.GetType(AuthenticationProviderType.AssemblyQualifiedName)) as IAuthenticationProvider;
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

        bool IAuthenticationProvider.CanHandleAuthScheme(string scheme)
        {
            return false;
        }
    }
}
