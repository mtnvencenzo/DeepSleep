namespace Api.DeepSleep.Controllers.Authorization
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System.Linq;

    public class AuthorizationController
    {
        private readonly IApiRequestContextResolver contextResolver;

        public AuthorizationController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        public AuthorizationModel GetWithAuthorization()
        {
            return new AuthorizationModel
            {
                IsAuthorized = contextResolver.GetContext().Request.ClientAuthorizationInfo?.AuthResults?.All(a => a.IsAuthorized),
                AuthorizedBy = contextResolver.GetContext().Request.ClientAuthorizationInfo?.AuthorizedBy ?? AuthorizationType.None
            };
        }
    }

    public class AuthorizationModel
    {
        public string Value => "Test";

        public bool? IsAuthorized { get; set; }

        public AuthorizationType AuthorizedBy { get; set; }
    }
}
