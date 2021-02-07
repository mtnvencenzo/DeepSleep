namespace DeepSleep.Media
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class MediaSerializerWriteOverrides
    {
        /// <summary>Initializes a new instance of the <see cref="MediaSerializerWriteOverrides"/> class.</summary>
        /// <param name="formatters">The formatters.</param>
        public MediaSerializerWriteOverrides(IList<IDeepSleepMediaSerializer> formatters)
        {
            this.Formatters = formatters;
        }

        /// <summary>Gets the formatters.</summary>
        /// <value>The formatters.</value>
        public IList<IDeepSleepMediaSerializer> Formatters { get; private set; }
    }
}
