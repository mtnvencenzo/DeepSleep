namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Formatting;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.ApiRouteReadWriteConfigurationAttribute" />
    /// <seealso cref="global::DeepSleep.IApiRouteWriterResolver" />
    public class CustomApiRouteReadWriteConfigurationWithWriteResolver : ApiRouteReadWriteConfigurationAttribute, IApiRouteWriterResolver
    {
        /// <summary>Resolves the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        Task<FormatterWriteOverrides> IApiRouteWriterResolver.Resolve(IApiRequestContextResolver contextResolver)
        {
            var formatters = new List<IFormatStreamReaderWriter>
            {
                new PlainTextFormatStreamReaderWriter()
            };

            return Task.FromResult(new FormatterWriteOverrides(formatters));
        }
    }
}
