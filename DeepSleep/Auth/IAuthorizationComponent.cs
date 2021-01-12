namespace DeepSleep.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthorizationComponent : IAuthorizationProvider
    {
        /// <summary>Activates the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IAuthorizationProvider Activate(ApiRequestContext context);
    }
}
