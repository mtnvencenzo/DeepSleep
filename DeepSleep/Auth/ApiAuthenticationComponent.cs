namespace DeepSleep.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiAuthenticationComponent<T> : IAuthenticationComponent where T : IAuthenticationProvider
    {
        private IAuthenticationProvider provider;

        string IAuthenticationProvider.Realm { get; }
        string IAuthenticationProvider.Scheme { get; }

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
                            this.provider = context.RequestServices.GetService(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthenticationProvider;
                        }
                    }
                    catch { }

                    if (this.provider == null)
                    {
                        try
                        {
                            this.provider = Activator.CreateInstance(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthenticationProvider;
                        }
                        catch { }
                    }
                }
            }

            return this.provider;
        }

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

        bool IAuthenticationProvider.CanHandleAuthScheme(string scheme)
        {
            return false;
        }
    }
}
