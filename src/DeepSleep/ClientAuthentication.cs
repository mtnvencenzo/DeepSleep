namespace DeepSleep
{
    using DeepSleep.Auth;
    using System.Security.Principal;

    /// <summary>
    /// 
    /// </summary>
    public class ClientAuthentication
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string AuthValue { get; set; }

        /// <summary>Gets or sets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public string AuthScheme { get; set; }

        /// <summary>Gets or sets the authentication result.</summary>
        /// <value>The authentication result.</value>
        public AuthenticationResult AuthResult { get; set; }

        /// <summary>Gets or sets the authenticated by.</summary>
        /// <value>The authenticated by.</value>
        public AuthenticationType AuthenticatedBy { get; set; }

        /// <summary>Gets or sets the principal.</summary>
        /// <value>The principal.</value>
        public IPrincipal Principal { get; set; }
    }
}