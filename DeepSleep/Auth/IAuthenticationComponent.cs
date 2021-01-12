namespace DeepSleep.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthenticationComponent : IAuthenticationProvider
    {
        /// <summary>Activates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IAuthenticationProvider Activate(ApiRequestContext context);
    }
}
