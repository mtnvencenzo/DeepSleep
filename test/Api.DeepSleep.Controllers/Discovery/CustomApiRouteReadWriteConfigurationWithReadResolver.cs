namespace Api.DeepSleep.Controllers.Discovery
{
    using global::DeepSleep;
    using global::DeepSleep.Media;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.ApiRouteMediaSerializerConfigurationAttribute" />
    /// <seealso cref="global::DeepSleep.IApiRouteReaderResolver" />
    public class CustomApiRouteReadWriteConfigurationWithReadResolver : ApiRouteMediaSerializerConfigurationAttribute, IApiRouteReaderResolver
    {
        /// <summary>Resolves the specified API request context resolver.</summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public Task<MediaSerializerReadOverrides> Resolve(IServiceProvider serviceProvider)
        {
            var formatters = new List<IDeepSleepMediaSerializer>
            {
                new PlainTextFormatStreamReaderWriter()
            };

            return Task.FromResult(new MediaSerializerReadOverrides(formatters));
        }
    }
}
