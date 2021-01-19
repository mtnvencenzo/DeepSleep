namespace Api.DeepSleep.Controllers.Authorization
{
    using global::DeepSleep;
    using global::DeepSleep.Auth;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationController
    {
        private readonly IApiRequestContextResolver contextResolver;

        /// <summary>Initializes a new instance of the <see cref="AuthorizationController"/> class.</summary>
        /// <param name="contextResolver">The context resolver.</param>
        public AuthorizationController(IApiRequestContextResolver contextResolver)
        {
            this.contextResolver = contextResolver;
        }

        /// <summary>Gets the with authorization.</summary>
        /// <returns></returns>
        public AuthorizationModel GetWithAuthorization()
        {
            return new AuthorizationModel
            {
                IsAuthorized = contextResolver.GetContext().Request.ClientAuthorizationInfo?.AuthResults?.All(a => a.IsAuthorized),
                AuthorizedBy = contextResolver.GetContext().Request.ClientAuthorizationInfo?.AuthorizedBy ?? AuthorizationType.None
            };
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationModel
    {
        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value => "Test";

        /// <summary>Gets or sets the is authorized.</summary>
        /// <value>The is authorized.</value>
        public bool? IsAuthorized { get; set; }

        /// <summary>Gets or sets the authorized by.</summary>
        /// <value>The authorized by.</value>
        public AuthorizationType AuthorizedBy { get; set; }
    }
}
