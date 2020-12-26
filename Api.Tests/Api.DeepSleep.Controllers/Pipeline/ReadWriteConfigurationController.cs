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
                AcceptHeader = context.Request.Accept.Value,
                WriteableTypes = context.Configuration.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.Configuration.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride.Value
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
                AcceptHeader = context.Request.Accept.Value,
                WriteableTypes = context.Configuration.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.Configuration.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride.Value
                    : null
            };
        }

        public ReadWriteOverrideModel PostWithReadableTypesTextXml([BodyBound] ReadWriteOverrideModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var context = this.requestContextResolver.GetContext();

            return new ReadWriteOverrideModel
            {
                AcceptHeader = context.Request.Accept.Value,
                WriteableTypes = context.Configuration.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.Configuration.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride.Value
                    : null
            };
        }

        public string PostWithReadableTypesPlainText([BodyBound] string model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model;
        }

        public string PostWithReadableTypesAllPlusPlainText([BodyBound] string model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model;
        }

        public ReadWriteOverrideModel GetWithWriteableOverrides()
        {
            var context = this.requestContextResolver.GetContext();

            return new ReadWriteOverrideModel
            {
                AcceptHeader = context.Request.Accept.Value,
                WriteableTypes = context.Configuration.ReadWriteConfiguration.WriteableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.WriteableMediaTypes)
                    : null,
                ReadableTypes = context.Configuration.ReadWriteConfiguration.ReadableMediaTypes != null
                    ? string.Join(",", context.Configuration.ReadWriteConfiguration.ReadableMediaTypes)
                    : null,
                AcceptHeaderOverride = context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride != (null as AcceptHeader)
                    ? context.Configuration.ReadWriteConfiguration.AcceptHeaderOverride.Value
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
