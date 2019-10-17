namespace DeepSleep.Formatting
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class FormatterResult
    {
        /// <summary>Gets the type of the formatter.</summary>
        /// <value>The type of the formatter.</value>
        public string FormatterType { get; internal set; }

        /// <summary>Gets the formatter.</summary>
        /// <value>The formatter.</value>
        public IFormatStreamReaderWriter Formatter { get; internal set; }

        /// <summary>Gets the resolved types.</summary>
        /// <value>The resolved types.</value>
        public IEnumerable<string> ResolvedTypes { get; internal set; }
    }
}
