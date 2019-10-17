namespace DeepSleep.Auth
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary></summary>
    public class DefaultAuthenticationFactory : IAuthenticationFactory
    {
        #region Constructors & Initialization

        /// <summary>
        /// 
        /// </summary>
        public DefaultAuthenticationFactory()
        {
            Providers = new List<IAuthenticationProvider>();
        }

        #endregion

        /// <summary>
        /// The registered list of authentication providers
        /// </summary>
        protected List<IAuthenticationProvider> Providers
        {
            get;
            set;
        }

        /// <summary>Gets the provider.</summary>
        /// <param name="scheme">The authorization scheme.</param>
        /// <returns>The <see cref="IAuthenticationProvider"/>.</returns>
        public IAuthenticationProvider GetProvider(string scheme)
        {
            return Providers.Where(p => p.CanHandleAuthScheme(scheme)).FirstOrDefault();
        }

        /// <summary>
        /// Adds a provider to the list of available providers
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public IAuthenticationFactory AddProvider(IAuthenticationProvider provider)
        {
            Providers.Add(provider);
            return this;
        }

        /// <summary>Fors the each.</summary>
        /// <returns></returns>
        public IEnumerable<IAuthenticationProvider> GetProviders()
        {
            return Providers;
        }

        /// <summary>Firsts the or default.</summary>
        /// <returns></returns>
        public IAuthenticationProvider FirstOrDefault()
        {
            return Providers.FirstOrDefault();
        }
    }
}