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

        /// <summary>Authorizes the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public async Task<AuthorizationResult> Authorize(IApiRequestContextResolver contextResolver)
        {
            var provider = this.Activate(contextResolver);
            if (provider != null)
            {
                return await provider.Authorize(contextResolver).ConfigureAwait(false);
            }

            return null;
        }

        /// <summary>Activates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        protected IAuthorizationProvider Activate(IApiRequestContextResolver contextResolver)
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
