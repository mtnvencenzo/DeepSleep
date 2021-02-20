namespace DeepSleep.Discovery
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Discovery.IDeepSleepSingleRouteRegistrationProvider" />
    public abstract class DeepSleepSingleRouteRegistrationProvider : IDeepSleepSingleRouteRegistrationProvider
    {
        private readonly DeepSleepRouteRegistration registration;

        /// <summary>Initializes a new instance of the <see cref="DeepSleepSingleRouteRegistrationProvider"/> class.</summary>
        /// <param name="registration">The registration.</param>
        /// <exception cref="System.ArgumentNullException">registration</exception>
        public DeepSleepSingleRouteRegistrationProvider(DeepSleepRouteRegistration registration)
        {
            this.registration = registration ?? throw new ArgumentNullException(nameof(registration));
        }

        /// <summary>Gets the route registration.</summary>
        /// <returns></returns>
        public DeepSleepRouteRegistration GetRouteRegistration()
        {
            return this.registration;
        }
    }
}
