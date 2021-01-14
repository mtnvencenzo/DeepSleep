namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="DeepSleep.Auth.IAuthenticationComponent" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthenticationAttribute : Attribute, IAuthenticationComponent
    {
        private IAuthenticationProvider provider;

        string IAuthenticationProvider.Realm { get; }
        string IAuthenticationProvider.Scheme { get; }

        /// <summary>Initializes a new instance of the <see cref="ApiAuthenticationAttribute"/> class.</summary>
        /// <param name="authenticationProviderType">Type of the authentication provider.</param>
        /// <exception cref="ArgumentNullException">validatorType</exception>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>Gets the type of the authentication provider.</summary>
        /// <value>The type of the authentication provider.</value>
        public Type AuthenticationProviderType { get; }

        /// <summary>Authenticates the specified API request context resolver.</summary>
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

        /// <summary>Activates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
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
