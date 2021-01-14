namespace DeepSleep
{
    using DeepSleep.Formatting;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRouteReaderResolver
    {
        /// <summary>Resolves the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        Task<FormatterReadOverrides> Resolve(IApiRequestContextResolver contextResolver);
    }
}
