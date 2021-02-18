namespace DeepSleep
{
    using DeepSleep.Discovery;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRoutingTable
    {
        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        IList<ApiRoutingItem> GetRoutes();

        /// <summary>Adds the route.</summary>
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        IApiRoutingTable AddRoute(DeepSleepRouteRegistration registration);

        /// <summary>Adds the routes.</summary>
        /// <param name="registrations">The registrations.</param>
        /// <returns></returns>
        IApiRoutingTable AddRoutes(IList<DeepSleepRouteRegistration> registrations);
    }
}
