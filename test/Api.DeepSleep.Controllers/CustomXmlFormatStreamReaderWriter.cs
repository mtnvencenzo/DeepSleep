namespace Api.DeepSleep.Controllers
{
    using global::DeepSleep.Configuration;
    using global::DeepSleep.Media.Serializers;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::DeepSleep.Media.Serializers.DeepSleepXmlMediaSerializer" />
    public class CustomXmlFormatStreamReaderWriter : DeepSleepXmlMediaSerializer
    {
        /// <summary>Initializes a new instance of the <see cref="CustomXmlFormatStreamReaderWriter"/> class.</summary>
        /// <param name="xmlFormattingConfiguration">The XML formatting configuration.</param>
        public CustomXmlFormatStreamReaderWriter(XmlMediaSerializerConfiguration xmlFormattingConfiguration)
            : base(xmlFormattingConfiguration)
        {
        }

        /// <summary>Gets the readable media types.</summary>
        /// <value>The readable media types.</value>
        public override IList<string> ReadableMediaTypes => new string[] { "application/xml", "other/xml" };

        /// <summary>Gets or sets the writeable media types.</summary>
        /// <value>The writeable media types.</value>
        public override IList<string> WriteableMediaTypes => new string[] { "application/xml", "other/xml" };
    }
}
