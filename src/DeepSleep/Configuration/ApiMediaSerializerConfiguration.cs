namespace DeepSleep.Configuration
{
    using DeepSleep.Media;
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiMediaSerializerConfiguration
    {
        /// <summary>Gets or sets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public IList<string> ReadableMediaTypes { get; set; }

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public IList<string> WriteableMediaTypes { get; set; }

        /// <summary>Gets or sets the accept header override.</summary>
        /// <value>The accept header override.</value>
        public AcceptHeader AcceptHeaderOverride { get; set; }

        /// <summary>Gets or sets the accept header fallback.</summary>
        /// <value>The accept header fallback.</value>
        public AcceptHeader AcceptHeaderFallback { get; set; }

        /// <summary>Gets or sets the reader resolver.</summary>
        /// <value>The reader resolver.</value>
        [JsonIgnore]
        public Func<IServiceProvider, Task<MediaSerializerReadOverrides>> ReaderResolver { get; set; }

        /// <summary>Gets or sets the writer resolver.</summary>
        /// <value>The writer resolver.</value>
        [JsonIgnore]
        public Func<IServiceProvider, Task<MediaSerializerWriteOverrides>> WriterResolver { get; set; }
    }
}
