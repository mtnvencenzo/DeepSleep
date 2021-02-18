namespace DeepSleep
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApiRequestContextResolver
    {
        /// <summary>Gets the context.</summary>
        /// <returns></returns>
        ApiRequestContext GetContext();

        /// <summary>Sets the context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IApiRequestContextResolver SetContext(ApiRequestContext context);
    }
}