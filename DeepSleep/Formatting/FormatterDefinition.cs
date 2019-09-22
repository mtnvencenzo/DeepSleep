using System.Collections.Generic;

namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    internal class FormatterDefinition
    {
        /// <summary>Gets or sets the types.</summary>
        /// <value>The types.</value>
        internal IEnumerable<string> Types { get; set; }

        /// <summary>Gets or sets the formatter.</summary>
        /// <value>The formatter.</value>
        internal IFormatStreamReaderWriter Formatter { get; set; }

        /// <summary>Gets or sets the charsets.</summary>
        /// <value>The charsets.</value>
        internal IEnumerable<string> Charsets { get; set; }
    }
}
