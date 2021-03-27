namespace Samples.Simple.Api
{
    using System.Collections.Generic;
    using System.Security.Principal;

    public class SecurityContextPrincipal : System.Security.Principal.IPrincipal
    {
        public SecurityContextPrincipal(int userId, IList<string> roles, string authenticationType)
        {
            this.UserId = userId;
            this.Roles = roles ?? new List<string>();
            this.Identity = new SecurityContextIdentity(authenticationType, true, userId.ToString());
        }

        public IIdentity Identity { get; }
        public int UserId { get; }
        public IList<string> Roles { get; }
        public virtual bool IsInRole(string role) => this.Roles.Contains(role);
    }
}
