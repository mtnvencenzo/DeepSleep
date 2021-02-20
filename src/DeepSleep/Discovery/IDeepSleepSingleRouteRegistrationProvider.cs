namespace DeepSleep.Discovery
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDeepSleepSingleRouteRegistrationProvider
    {
        /// <summary>Gets the route registration.</summary>
        /// <returns></returns>
        DeepSleepRouteRegistration GetRouteRegistration();
    }
}
