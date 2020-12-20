using System;

namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    public class ResolvedFormatterArguments
    {
        /// <summary>Initializes a new instance of the <see cref="ResolvedFormatterArguments"/> class.</summary>
        /// <param name="context">The context.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="options">The options.</param>
        public ResolvedFormatterArguments(ApiRequestContext context, IFormatStreamReaderWriter formatter, IFormatStreamOptions options = null)
        {
            this.Context = context;
            this.Formatter = formatter;
            this.Options = options;
        }

        /// <summary>Gets the context.</summary>
        /// <value>The context.</value>
        public ApiRequestContext Context { get; private set; }

        /// <summary>Gets the formatter.</summary>
        /// <value>The formatter.</value>
        public IFormatStreamReaderWriter Formatter { get; private set; }

        /// <summary>Gets the options.</summary>
        /// <value>The options.</value>
        public IFormatStreamOptions Options { get; private set; }
    }
}
