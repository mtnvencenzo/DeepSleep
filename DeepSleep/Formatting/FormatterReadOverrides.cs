using System.Collections.Generic;

namespace DeepSleep.Formatting
{
    /// <summary>
    /// 
    /// </summary>
    public class FormatterReadOverrides
    {
        /// <summary>Initializes a new instance of the <see cref="FormatterReadOverrides"/> class.</summary>
        /// <param name="formatters">The formatters.</param>
        public FormatterReadOverrides(IList<IFormatStreamReaderWriter> formatters)
        {
            this.Formatters = formatters;
        }

        /// <summary>Gets the formatter.</summary>
        /// <value>The formatter.</value>
        public IList<IFormatStreamReaderWriter> Formatters { get; private set; }
    }
}
