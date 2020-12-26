namespace DeepSleep.Configuration
{
    using DeepSleep.Formatting;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class ApiReadWriteConfiguration
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

        /// <summary>Gets or sets the reader resolver.</summary>
        /// <value>The reader resolver.</value>
        public Func<ResolvedFormatterArguments, Task<FormatterReadOverrides>> ReaderResolver { get; set; }

        /// <summary>Gets or sets the writer resolver.</summary>
        /// <value>The writer resolver.</value>
        public Func<ResolvedFormatterArguments, Task<FormatterWriteOverrides>> WriterResolver { get; set; }
    }
}
