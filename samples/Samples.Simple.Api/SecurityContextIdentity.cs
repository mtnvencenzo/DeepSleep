namespace Samples.Simple.Api
{
    public class SecurityContextIdentity : System.Security.Principal.IIdentity
    {
        public SecurityContextIdentity(string authenticationType, bool isAuthenticated, string name)
        {
            this.AuthenticationType = authenticationType;
            this.IsAuthenticated = isAuthenticated;
            this.Name = name;
        }

        public string AuthenticationType { get; }
        public bool IsAuthenticated { get; }
        public string Name { get; }
    }
}
