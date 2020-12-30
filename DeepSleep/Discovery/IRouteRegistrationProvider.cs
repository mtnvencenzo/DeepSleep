namespace DeepSleep.Discovery
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IRouteRegistrationProvider
    {
        /// <summary>Gets the routes.</summary>
        /// <returns></returns>
        Task<IList<ApiRouteRegistration>> GetRoutes();
    }
}
