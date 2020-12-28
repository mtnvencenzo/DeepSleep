namespace Api.DeepSleep.Controllers.Authorization
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;

    public class AuthorizationController
    {
        private readonly IApiRequestContextResolver apiRequestContextResolver;

        public AuthorizationController(IApiRequestContextResolver apiRequestContextResolver)
        {
            this.apiRequestContextResolver = apiRequestContextResolver;
        }

        public AuthorizationModel GetWithAuthorization()
        {
            return new AuthorizationModel
            {
                IsAuthorized = apiRequestContextResolver.GetContext().Request.ClientAuthorizationInfo?.AuthResult?.IsAuthorized,
                AuthorizedBy = apiRequestContextResolver.GetContext().Request.ClientAuthorizationInfo?.AuthorizedBy ?? AuthorizationType.None
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
