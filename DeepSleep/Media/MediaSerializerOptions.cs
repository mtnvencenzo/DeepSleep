namespace DeepSleep.Media
{
    using System.Globalization;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Media.IMediaSerializerOptions" />
    public class MediaSerializerOptions : IMediaSerializerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaSerializerOptions"/> class.
        /// </summary>
        public MediaSerializerOptions()
        {
            Encoding = Encoding.UTF8;
            Culture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>Gets or sets the encoding.</summary>
        /// <value>The encoding.</value>
        [JsonIgnore]
        public Encoding Encoding { get; set; }

        /// <summary>Gets or sets the culture.</summary>
        /// <value>The culture.</value>
        [JsonIgnore]
        public CultureInfo Culture { get; set; }
    }
}
