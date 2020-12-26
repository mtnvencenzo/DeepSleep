namespace Api.DeepSleep.Controllers.Binding
{
    public class AuthenticationController
    {
        public AuthenticatedModel GetWithSingleSupportedScheme()
        {
            return new AuthenticatedModel();
        }

        public AuthenticatedModel GetWithMultipleSupportedScheme()
        {
            return new AuthenticatedModel();
        }

        public AuthenticatedModel GetWithNoSupportedScheme()
        {
            return new AuthenticatedModel();
        }

        public AuthenticatedModel GetWithSupportedSchemesNotDefined()
        {
            return new AuthenticatedModel();
        }

        public AuthenticatedModel GetWithAllowAnonymousTrue()
        {
            return new AuthenticatedModel();
        }
    }

    public class AuthenticatedModel
    {
        public string Value => "Test";
    }
}
