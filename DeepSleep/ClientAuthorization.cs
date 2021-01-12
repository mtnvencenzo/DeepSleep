namespace DeepSleep
{
    using DeepSleep.Auth;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class ClientAuthorization
    {
        /// <summary>Gets or sets the authentication results.</summary>
        /// <value>The authentication results.</value>
        public IList<AuthorizationResult> AuthResults { get; } = new List<AuthorizationResult>();

        /// <summary>Gets or sets the authorized by.</summary>
        /// <value>The authorized by.</value>
        public AuthorizationType AuthorizedBy { get; set; }
    }
}