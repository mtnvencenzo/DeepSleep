namespace DeepSleep.OpenApi
{
    using DeepSleep.Discovery;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.DeepSleepSingleRouteRegistrationProvider" />
    internal class OasV3RouteRegistrationProvider : DeepSleepSingleRouteRegistrationProvider
    {
        /// <summary>Initializes a new instance of the <see cref="OasV3RouteRegistrationProvider"/> class.</summary>
        /// <param name="registration">The registration.</param>
        internal OasV3RouteRegistrationProvider(DeepSleepRouteRegistration registration)
            : base(registration)
        {
        }
    }
}
