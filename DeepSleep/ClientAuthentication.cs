using DeepSleep.Auth;
using System.Security.Principal;

namespace DeepSleep
{
    /// <summary></summary>
    public class ClientAuthentication
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string AuthValue { get; set; }

        /// <summary>Gets or sets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public string AuthScheme { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuthenticationResult AuthResult { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuthenticationType AuthenticatedBy { get; set; }

        /// <summary>Gets or sets the authenticated security principal.
        /// </summary>
        public IPrincipal Principal { get; set; }
    }
}