namespace DeepSleep.Auth
{
    /// <summary>Defines the method for which a request was authorized.
    /// </summary>
    public enum AuthorizationType
    {
        /// <summary>
        /// Authorization was not attempted.
        /// </summary>
        /// <remarks>
        /// A value of <see cref="None"/> is common for requests that were rejected prior to authorization was attempted. 
        /// <para>Or, when no <see cref="IAuthorizationProvider"/>(s) were available to authorize the request.</para>
        /// </remarks>
        None = 0,

        /// <summary>
        /// Authorization was attempted by an <see cref="IAuthorizationProvider"/>.
        /// </summary>
        Provider = 1,

        /// <summary>
        /// Authorization was bypassed due to the request configuration having <see cref="Configuration.IApiRequestConfiguration.AllowAnonymous"/> set to <c>true</c>.
        /// </summary>
        Anonymous = 2
    }
}
