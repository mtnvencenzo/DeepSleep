namespace DeepSleep.Auth
{
    /// <summary>Defines the method for which a request was authenticated.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// Authentication was not attempted.
        /// </summary>
        /// <remarks>
        /// A value of <see cref="None"/> is common for requests that were rejected prior to authentication was attempted. 
        /// <para>Or, when no <see cref="IAuthenticationProvider"/>(s) were available matching the Authorization header scheme to authenticate the request.</para>
        /// </remarks>
        None = 0,

        /// <summary>
        /// Authentication was attempted by an <see cref="IAuthenticationProvider"/>.
        /// </summary>
        Provider = 1,

        /// <summary>
        /// Authentication was bypassed due to the request configuration having <see cref="Configuration.IDeepSleepRequestConfiguration.AllowAnonymous"/> set to <c>true</c>.
        /// </summary>
        Anonymous = 2
    }
}
