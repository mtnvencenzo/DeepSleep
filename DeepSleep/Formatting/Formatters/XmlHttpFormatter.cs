namespace DeepSleep.Formatting.Formatters
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DeepSleep.Formatting.IFormatStreamReaderWriter" />
    public class XmlHttpFormatter : IFormatStreamReaderWriter
    {
        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <returns></returns>
        public virtual Task<object> ReadType(Stream stream, Type objType)
        {
            return ReadType(stream, objType, new FormatterOptions());
        }

        /// <summary>Reads the type.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="objType">Type of the object.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public virtual Task<object> ReadType(Stream stream, Type objType, IFormatStreamOptions options)
        {
            object obj = null;
            var serializer = new XmlSerializer(objType);
            var settings = new XmlReaderSettings
            {
                CloseInput = false,
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreComments = true,
                ValidationType = ValidationType.None
            };

            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                obj = serializer.Deserialize(reader);
            }

            return Task.FromResult(obj);
        }

        /// <summary>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="preWriteCallback"></param>
        /// <returns></returns>
        public virtual async Task<long> WriteType(Stream stream, object obj, Action<long> preWriteCallback = null)
        {
            return await WriteType(stream, obj, new FormatterOptions(), preWriteCallback).ConfigureAwait(false);
        }

        /// <summary>
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <param name="preWriteCallback"></param>
        /// <returns></returns>
        public virtual async Task<long> WriteType(Stream stream, object obj, IFormatStreamOptions options, Action<long> preWriteCallback = null)
        {
            long length = 0;

            if (obj != null)
            {
                var serializer = new XmlSerializer(obj.GetType());
                var settings = new XmlWriterSettings
                {
                    NewLineOnAttributes = false,
                    CloseOutput = false,
                    Encoding = options.Encoding,
                    Indent = options.PrettyPrint,
                    NamespaceHandling = NamespaceHandling.Default,
                    OmitXmlDeclaration = true,
                    WriteEndDocumentOnClose = false,
                    Async = true,
                };

                using (var ms = new MemoryStream())
                using (var writer = XmlWriter.Create(ms, settings))
                {
                    serializer.Serialize(writer, obj);
                    writer.Flush();
                    length = ms.Length;
                    ms.Seek(0, SeekOrigin.Begin);

                    preWriteCallback?.Invoke(length);

                    await ms.CopyToAsync(stream).ConfigureAwait(false);
                }
            }

            return length;
        }

        /// <summary>
        /// Gets a value indicating whether [supports pretty print].
        /// </summary>
        /// <value><c>true</c> if [supports pretty print]; otherwise, <c>false</c>.</value>
        public virtual bool SupportsPrettyPrint => true;

        /// <summary>Whether the formatter can read content
        /// </summary>
        public virtual bool SupportsRead => true;

        /// <summary>Whether the formatter can write content
        /// </summary>
        public virtual bool SupportsWrite => true;

        /// <summary>
        /// 
        /// </summary>
        public virtual IList<string> SuuportedContentTypes => new string[] { "text/xml", "application/xml" };

        /// <summary>
        /// 
        /// </summary>
        public virtual IList<string> SuuportedCharsets => new string[] { "utf-32, utf-16, utf-8" };
    }
}
