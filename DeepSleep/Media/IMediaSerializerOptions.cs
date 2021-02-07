namespace DeepSleep.Media
{
    using System.Globalization;
    using System.Text;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public interface IMediaSerializerOptions
    {
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
