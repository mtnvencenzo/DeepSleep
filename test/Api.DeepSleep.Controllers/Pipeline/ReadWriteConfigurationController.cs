namespace Api.DeepSleep.Controllers.Pipeline
{
    using global::DeepSleep;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class ReadWriteConfigurationController
    {
        private readonly IApiRequestContextResolver requestContextResolver;

        /// <summary>Initializes a new instance of the <see cref="ReadWriteConfigurationController"/> class.</summary>
        /// <param name="requestContextResolver">The request context resolver.</param>
        public ReadWriteConfigurationController(IApiRequestContextResolver requestContextResolver)
        {
            this.requestContextResolver = requestContextResolver;
        }

        /// <summary>Gets the with accept override.</summary>
        /// <returns></returns>
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

        /// <summary>Gets the with406 accept override.</summary>
        /// <returns></returns>
        /// <exception cref="Exception">Shouold not have been called</exception>
        public ReadWriteOverrideModel GetWith406AcceptOverride()
        {
            throw new Exception("Shouold not have been called");
        }

        /// <summary>Gets the with writeable types text XML.</summary>
        /// <returns></returns>
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

        /// <summary>Posts the with readable types text XML.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">model</exception>
        public ReadWriteOverrideModel PostWithReadableTypesTextXml([InBody] ReadWriteOverrideModel model)
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

        /// <summary>Posts the with readable types plain text.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">model</exception>
        public string PostWithReadableTypesPlainText([InBody] string model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model;
        }

        /// <summary>Posts the with readable types all plus plain text.</summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">model</exception>
        public string PostWithReadableTypesAllPlusPlainText([InBody] string model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return model;
        }

        /// <summary>Gets the with writeable overrides.</summary>
        /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    public class ReadWriteOverrideModel
    {
        /// <summary>Gets or sets the accept header.</summary>
        /// <value>The accept header.</value>
        public string AcceptHeader { get; set; }
        /// <summary>Gets or sets the accept header override.</summary>
        /// <value>The accept header override.</value>
        public string AcceptHeaderOverride { get; set; }
        /// <summary>Gets or sets the readable types.</summary>
        /// <value>The readable types.</value>
        public string ReadableTypes { get; set; }
        /// <summary>Gets or sets the writeable types.</summary>
        /// <value>The writeable types.</value>
        public string WriteableTypes { get; set; }
    }
}
