namespace Api.DeepSleep.Controllers.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationController
    {
        /// <summary>Gets the with single supported scheme.</summary>
        /// <returns></returns>
        public AuthenticatedModel GetWithSingleSupportedScheme()
        {
            return new AuthenticatedModel();
        }

        /// <summary>Gets the with multiple supported scheme.</summary>
        /// <returns></returns>
        public AuthenticatedModel GetWithMultipleSupportedScheme()
        {
            return new AuthenticatedModel();
        }

        /// <summary>Gets the with no supported scheme.</summary>
        /// <returns></returns>
        public AuthenticatedModel GetWithNoSupportedScheme()
        {
            return new AuthenticatedModel();
        }

        /// <summary>Gets the with supported schemes not defined.</summary>
        /// <returns></returns>
        public AuthenticatedModel GetWithSupportedSchemesNotDefined()
        {
            return new AuthenticatedModel();
        }

        /// <summary>Gets the with allow anonymous true.</summary>
        /// <returns></returns>
        public AuthenticatedModel GetWithAllowAnonymousTrue()
        {
            return new AuthenticatedModel();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AuthenticatedModel
    {
        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        public string Value => "Test";
    }
}
