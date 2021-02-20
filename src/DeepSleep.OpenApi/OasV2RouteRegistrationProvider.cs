namespace DeepSleep.OpenApi
{
    using DeepSleep.Discovery;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.DeepSleepSingleRouteRegistrationProvider" />
    internal class OasV2RouteRegistrationProvider : DeepSleepSingleRouteRegistrationProvider
    {
        /// <summary>Initializes a new instance of the <see cref="OasV2RouteRegistrationProvider"/> class.</summary>
        /// <param name="registration">The registration.</param>
        internal OasV2RouteRegistrationProvider(DeepSleepRouteRegistration registration)
            : base(registration)
        {
        }
    }
}
