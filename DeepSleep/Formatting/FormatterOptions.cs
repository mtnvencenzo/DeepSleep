namespace DeepSleep.Formatting
{
    using System.Globalization;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamOptions" />
    public class FormatterOptions : IFormatStreamOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatterOptions"/> class.
        /// </summary>
        public FormatterOptions()
        {
            Encoding = Encoding.UTF8;
            Culture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [pretty print].
        /// </summary>
        /// <value><c>true</c> if [pretty print]; otherwise, <c>false</c>.</value>
        public bool PrettyPrint { get; set; }

        /// <summary>Gets or sets the encoding.</summary>
        /// <value>The encoding.</value>
        public Encoding Encoding { get; set; }

        /// <summary>Gets or sets the culture.</summary>
        /// <value>The culture.</value>
        public CultureInfo Culture { get; set; }
    }
}
