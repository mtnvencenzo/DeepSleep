namespace DeepSleep.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.IRouteDiscoveryStrategy" />
    public class DelegatedRouteDiscoveryStrategy : IRouteDiscoveryStrategy
    {
        /// <summary>The assembly directory path</summary>
        protected readonly string assemblyDirectoryPath;
        /// <summary>The assembly match pattern</summary>
        protected readonly string assemblyMatchPattern;
        /// <summary>The search option</summary>
        protected readonly SearchOption searchOption;

        /// <summary>Initializes a new instance of the <see cref="DelegatedRouteDiscoveryStrategy" /> class.</summary>
        /// <param name="assemblyDirectoryPath">The assembly directory path.</param>
        /// <param name="assemblyMatchPattern">The assembly match pattern.</param>
        /// <param name="searchOption">The search option.</param>
        public DelegatedRouteDiscoveryStrategy(
            string assemblyDirectoryPath, 
            string assemblyMatchPattern = null, 
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            this.assemblyDirectoryPath = assemblyDirectoryPath;

            this.assemblyMatchPattern = assemblyMatchPattern ?? "*";

            this.searchOption = searchOption;
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public virtual async Task<IList<ApiRouteRegistration>> DiscoverRoutes(IServiceProvider serviceProvider)
        {
            if (this.assemblyDirectoryPath != null)
            {
                return await this.DiscoverRoutes(serviceProvider, this.assemblyDirectoryPath, this.assemblyMatchPattern).ConfigureAwait(false);
            }

            return new List<ApiRouteRegistration>();
        }

        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="assemblyDirectoryPath">The assembly directory path.</param>
        /// <param name="assemblyMatchPattern">The assembly match pattern.</param>
        /// <returns></returns>
        protected virtual async Task<IList<ApiRouteRegistration>> DiscoverRoutes(
            IServiceProvider serviceProvider, 
            string assemblyDirectoryPath, 
            string assemblyMatchPattern)
        {
            var registrations = new List<ApiRouteRegistration>();

            var files = Directory.GetFiles(assemblyDirectoryPath, assemblyMatchPattern, this.searchOption);

            foreach (var file in files)
            {
                Assembly assembly = null;

                try
                {
                    assembly = Assembly.LoadFile(file);
                }
                catch { }


                if (assembly != null)
                {
                    var types = assembly
                        .GetTypes()
                        .Where(t => t.GetInterface(nameof(IRouteRegistrationProvider)) != null);

                    foreach (var type in types)
                    {
                        IRouteRegistrationProvider instance = null;

                        var fullyQualifiedType = Type.GetType(type.AssemblyQualifiedName);

                        instance = serviceProvider?.GetService(fullyQualifiedType) as IRouteRegistrationProvider;

                        if (instance == null)
                        {
                            try
                            {
                                instance = Activator.CreateInstance(fullyQualifiedType) as IRouteRegistrationProvider;
                            }
                            catch { }
                        }

                        if (instance != null)
                        {
                            var instanceRegistrations = await instance.GetRoutes(serviceProvider).ConfigureAwait(false);

                            if (instanceRegistrations != null)
                            {
                                foreach (var registration in instanceRegistrations)
                                {
                                    if (registration == null)
                                    {
                                        continue;
                                    }

                                    registrations.Add(registration);
                                }
                            }
                        }
                    }
                }
            }

            return registrations;

        }
    }
}
