namespace DeepSleep
{
    using DeepSleep.Media;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IApiRouteWriterResolver
    {
        /// <summary>Resolves the specified API request context resolver.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        Task<MediaSerializerWriteOverrides> Resolve(IServiceProvider serviceProvider);
    }
}
