namespace DeepSleep.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IRouteDiscoveryStrategy
    {
        /// <summary>Discovers the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        Task<IList<ApiRouteRegistration>> DiscoverRoutes(IServiceProvider serviceProvider);
    }
}
