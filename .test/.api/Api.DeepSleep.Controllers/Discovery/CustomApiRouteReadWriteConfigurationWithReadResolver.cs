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
    /// <seealso cref="global::DeepSleep.IApiRouteReaderResolver" />
    public class CustomApiRouteReadWriteConfigurationWithReadResolver : ApiRouteReadWriteConfigurationAttribute, IApiRouteReaderResolver
    {
        /// <summary>Resolves the specified API request context resolver.</summary>
        /// <param name="contextResolver">The API request context resolver.</param>
        /// <returns></returns>
        public Task<FormatterReadOverrides> Resolve(IApiRequestContextResolver contextResolver)
        {
            var formatters = new List<IFormatStreamReaderWriter>
            {
                new PlainTextFormatStreamReaderWriter()
            };

            return Task.FromResult(new FormatterReadOverrides(formatters));
        }
    }
}
