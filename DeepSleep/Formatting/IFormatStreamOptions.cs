namespace DeepSleep.Formatting
{
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public interface IFormatStreamOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [pretty print].
        /// </summary>
        /// <value><c>true</c> if [pretty print]; otherwise, <c>false</c>.</value>
        bool PrettyPrint { get; set; }

        /// <summary>Gets or sets the encoding.</summary>
        /// <value>The encoding.</value>
        Encoding Encoding { get; set; }

        /// <summary>Gets or sets the culture.</summary>
        /// <value>The culture.</value>
        CultureInfo Culture { get; set; }
    }
}
