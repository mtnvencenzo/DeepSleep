using DeepSleep.Auth;
using System.Collections.Generic;

namespace DeepSleep
{
    /// <summary></summary>
    public class ClientAuthorization
    {
        /// <summary>Gets or sets the authentication value.</summary>
        /// <value>The authentication value.</value>
        public string Policy { get; set; }

        /// <summary>Gets or sets the authentication scheme.</summary>
        /// <value>The authentication scheme.</value>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuthorizationResult AuthResult { get; set; }
    }
}