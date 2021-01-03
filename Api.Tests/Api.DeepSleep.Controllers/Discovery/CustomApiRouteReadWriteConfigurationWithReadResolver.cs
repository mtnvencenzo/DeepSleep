namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Formatting;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CustomApiRouteReadWriteConfigurationWithReadResolver : ApiRouteReadWriteConfigurationAttribute, IApiRouteReaderResolver
    {
        public Task<FormatterReadOverrides> Resolve(ResolvedFormatterArguments args)
        {
            var formatters = new List<IFormatStreamReaderWriter>
            {
                new PlainTextFormatStreamReaderWriter()
            };

            return Task.FromResult(new FormatterReadOverrides(formatters));
        }
    }
}
