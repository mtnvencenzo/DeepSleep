namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Formatting;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomApiRouteReadWriteConfigurationWithWriteResolver : ApiRouteReadWriteConfigurationAttribute, IApiRouteWriterResolver
    {
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
