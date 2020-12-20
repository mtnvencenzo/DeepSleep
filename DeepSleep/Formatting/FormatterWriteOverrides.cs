namespace DeepSleep.Formatting
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class FormatterWriteOverrides
    {
        /// <summary>Initializes a new instance of the <see cref="FormatterWriteOverrides"/> class.</summary>
        /// <param name="formatters">The formatters.</param>
        public FormatterWriteOverrides(IList<IFormatStreamReaderWriter> formatters)
        {
            this.Formatters = formatters;
        }

        /// <summary>Gets the formatters.</summary>
        /// <value>The formatters.</value>
        public IList<IFormatStreamReaderWriter> Formatters { get; private set; }
    }
}
