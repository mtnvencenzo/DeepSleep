namespace DeepSleep
{
    using DeepSleep.Formatting;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRouteWriterResolver
    {
        /// <summary>Resolves the specified arguments.</summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        Task<FormatterWriteOverrides> Resolve(ResolvedFormatterArguments args);
    }
}
