namespace DeepSleep.Configuration
{
    using System.Xml;

    /// <summary>
    /// 
    /// </summary>
    public class XmlMediaSerializerConfiguration
    {
        /// <summary>Gets or sets the reader serializer settings.</summary>
        /// <value>The reader serializer settings.</value>
        public XmlReaderSettings ReaderSerializerSettings { get; set; }

        /// <summary>Gets or sets the writer serializer settings.</summary>
        /// <value>The writer serializer settings.</value>
        public XmlWriterSettings WriterSerializerSettings { get; set; }
    }
}
