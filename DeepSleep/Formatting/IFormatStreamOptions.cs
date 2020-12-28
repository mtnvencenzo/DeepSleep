namespace DeepSleep.Formatting
{
    using System.Globalization;
    using System.Text;
    using System.Text.Json.Serialization;

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
        [JsonIgnore]
        Encoding Encoding { get; set; }

        /// <summary>Gets or sets the culture.</summary>
        /// <value>The culture.</value>
        [JsonIgnore]
        CultureInfo Culture { get; set; }
    }
}
