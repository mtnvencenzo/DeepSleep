namespace DeepSleep.Auth
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiAuthorizationComponent<T> : IAuthorizationComponent where T : IAuthorizationProvider
    {
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
