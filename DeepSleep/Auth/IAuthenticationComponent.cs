namespace DeepSleep.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthenticationComponent : IAuthenticationProvider
    {
        /// <summary>Activates the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        IAuthenticationProvider Activate(IApiRequestContextResolver contextResolver);
    }
}
