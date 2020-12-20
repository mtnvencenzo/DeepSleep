namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;
    using System;

    public class ReadWriteConfigurationController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        public ReadWriteConfigurationController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        public ReadWriteOverrideModel GetWithAcceptOverride()
        {
            var context = this.requestContextResolver.GetContext();

            return new ReadWriteOverrideModel
            {
                AcceptHeader = context.RequestInfo.Accept.Value,
                WriteableTypes = context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride.Value
                    : null
            };
        }

        public ReadWriteOverrideModel GetWith406AcceptOverride()
        {
            throw new Exception("Shouold not have been called");
        }

        public ReadWriteOverrideModel GetWithWriteableTypesTextXml()
        {
            var context = this.requestContextResolver.GetContext();

            return new ReadWriteOverrideModel
            {
                AcceptHeader = context.RequestInfo.Accept.Value,
                WriteableTypes = context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride.Value
                    : null
            };
        }

        public ReadWriteOverrideModel PostWithReadableTypesTextXml([BodyBound] ReadWriteOverrideModel model)
        {
            var context = this.requestContextResolver.GetContext();

            return new ReadWriteOverrideModel
            {
                AcceptHeader = context.RequestInfo.Accept.Value,
                WriteableTypes = context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride.Value
                    : null
            };
        }

        public ReadWriteOverrideModel GetWithWriteableOverrides()
        {
            var context = this.requestContextResolver.GetContext();

            return new ReadWriteOverrideModel
            {
                AcceptHeader = context.RequestInfo.Accept.Value,
                WriteableTypes = context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.RequestConfig.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.RequestConfig.ReadWriteConfiguration.AcceptHeaderOverride.Value
                    : null
            };
        }
    }

    public class ReadWriteOverrideModel
    {
        public string AcceptHeader { get; set; }
        public string AcceptHeaderOverride { get; set; }
        public string ReadableTypes { get; set; }
        public string WriteableTypes { get; set; }
    }
}
