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

        /// <summary>Authenticates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task<AuthenticationResult> Authenticate(ApiRequestContext context)
        {
            if (context != null)
            {
                var provider = this.Activate(context);
                if (provider != null)
                {
                    return await provider.Authenticate(context).ConfigureAwait(false);
                }
            }

            return null;
        }

        /// <summary>Activates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public IAuthenticationProvider Activate(ApiRequestContext context)
        {
            if (this.provider == null)
            {
                if (context != null)
                {
                    try
                    {
                        if (context.RequestServices != null)
                        {
                            this.provider = context.RequestServices.GetService(Type.GetType(AuthenticationProviderType.AssemblyQualifiedName)) as IAuthenticationProvider;
                        }
                    }
                    catch { }

                    if (this.provider == null)
                    {
                        try
                        {
                            this.provider = Activator.CreateInstance(Type.GetType(AuthenticationProviderType.AssemblyQualifiedName)) as IAuthenticationProvider;
                        }
                        catch { }
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
