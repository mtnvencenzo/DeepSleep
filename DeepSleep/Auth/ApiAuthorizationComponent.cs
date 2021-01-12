namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiAuthorizationComponent<T> : IAuthorizationComponent where T : IAuthorizationProvider
    {
        private IAuthorizationProvider provider;

        string IAuthorizationProvider.Policy { get; }

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
                            this.provider = context.RequestServices.GetService(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthorizationProvider;
                        }
                    }
                    catch { }

                    if (this.provider == null)
                    {
                        try
                        {
                            this.provider = Activator.CreateInstance(Type.GetType(typeof(T).AssemblyQualifiedName)) as IAuthorizationProvider;
                        }
                        catch { }
                    }
                }
            }

            return this.provider;
        }

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

        bool IAuthorizationProvider.CanHandleAuthPolicy(string policy)
        {
            return false;
        }
    }
}
