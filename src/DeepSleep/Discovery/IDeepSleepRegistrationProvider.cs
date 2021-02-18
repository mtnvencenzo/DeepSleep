namespace DeepSleep.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepRegistrationProvider
    {
        /// <summary>Gets the routes.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        Task<IList<DeepSleepRouteRegistration>> GetRoutes(IServiceProvider serviceProvider);
    }
}
