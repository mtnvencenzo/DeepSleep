namespace DeepSleep
{
    /// <summary></summary>
    public interface IApiRequestContextResolver
    {
        /// <summary>Gets the context.</summary>
        /// <returns></returns>
        ApiRequestContext GetContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IApiRequestContextResolver SetContext(ApiRequestContext context);
    }
}