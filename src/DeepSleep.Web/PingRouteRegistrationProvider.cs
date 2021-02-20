namespace DeepSleep.Web
{
    using DeepSleep.Discovery;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.DeepSleepSingleRouteRegistrationProvider" />
    internal class PingRouteRegistrationProvider : DeepSleepSingleRouteRegistrationProvider
    {
        /// <summary>Initializes a new instance of the <see cref="PingRouteRegistrationProvider"/> class.</summary>
        /// <param name="registration">The registration.</param>
        internal PingRouteRegistrationProvider(DeepSleepRouteRegistration registration)
            : base(registration)
        {
        }
    }
}
