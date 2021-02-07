namespace DeepSleep.Media
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class MediaSerializerReadOverrides
    {
        /// <summary>Initializes a new instance of the <see cref="MediaSerializerReadOverrides"/> class.</summary>
        /// <param name="formatters">The formatters.</param>
        public MediaSerializerReadOverrides(IList<IDeepSleepMediaSerializer> formatters)
        {
            this.Formatters = formatters;
        }

        /// <summary>Gets the formatter.</summary>
        /// <value>The formatter.</value>
        public IList<IDeepSleepMediaSerializer> Formatters { get; private set; }
    }
}
