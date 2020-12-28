namespace DeepSleep
{
    using DeepSleep.Auth;

    /// <summary>
    /// 
    /// </summary>
    public class ClientAuthorization
    {
        /// <summary>Gets or sets the authentication result.</summary>
        /// <value>The authentication result.</value>
        public AuthorizationResult AuthResult { get; set; }

        /// <summary>Gets or sets the authorized by.</summary>
        /// <value>The authorized by.</value>
        public AuthorizationType AuthorizedBy { get; set; }
    }
}