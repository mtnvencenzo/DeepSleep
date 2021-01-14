namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="DeepSleep.Auth.IAuthenticationComponent" />
    public class ApiAuthenticationComponent<T> : IAuthenticationComponent where T : IAuthenticationProvider
    {
        private IAuthenticationProvider provider;

        string IAuthenticationProvider.Realm { get; }
        string IAuthenticationProvider.Scheme { get; }

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

        bool IAuthenticationProvider.CanHandleAuthScheme(string scheme)
        {
            return false;
        }
    }
}
