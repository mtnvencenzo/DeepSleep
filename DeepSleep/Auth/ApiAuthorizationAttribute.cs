namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="DeepSleep.Auth.IAuthorizationComponent" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ApiAuthorizationAttribute : Attribute, IAuthorizationComponent
    {
        private IAuthorizationProvider provider;

        string IAuthorizationProvider.Policy { get; }

        /// <summary>Initializes a new instance of the <see cref="ApiAuthorizationAttribute"/> class.</summary>
        /// <param name="authorizationProviderType">Type of the authorization provider.</param>
        /// <exception cref="ArgumentNullException">authenticationProviderType</exception>
        /// <exception cref="ArgumentException"></exception>
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

        /// <summary>Gets the type of the authorization provider.</summary>
        /// <value>The type of the authorization provider.</value>
        public Type AuthorizationProviderType { get; }

        /// <summary>Authorizes the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task<AuthorizationResult> Authorize(ApiRequestContext context)
        {
            if (context != null)
            {
                var provider = this.Activate(context);
                if (provider != null)
                {
                    return await provider.Authorize(context).ConfigureAwait(false);
                }
            }

            return null;
        }

        /// <summary>Activates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public IAuthorizationProvider Activate(ApiRequestContext context)
        {
            if (this.provider == null)
            {
                if (context != null)
                {
                    try
                    {
                        if (context.RequestServices != null)
                        {
                            this.provider = context.RequestServices.GetService(Type.GetType(AuthorizationProviderType.AssemblyQualifiedName)) as IAuthorizationProvider;
                        }
                    }
                    catch { }

                    if (this.provider == null)
                    {
                        try
                        {
                            this.provider = Activator.CreateInstance(Type.GetType(AuthorizationProviderType.AssemblyQualifiedName)) as IAuthorizationProvider;
                        }
                        catch { }
                    }
                }
            }

            return this.provider;
        }

        bool IAuthorizationProvider.CanHandleAuthPolicy(string policy)
        {
            return false;
        }
    }
}
